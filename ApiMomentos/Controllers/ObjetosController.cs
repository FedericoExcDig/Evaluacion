using ApiObjetos.Data;
using ApiObjetos.Models;
using ApiObjetos.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiObjetos.Auth;

namespace ApiObjetos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Aplica la seguridad a todos los metodos globalmente

    public class ObjetosController : ControllerBase
    {
        private readonly ObjetosContext _db;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        public ObjetosController(ObjetosContext db, IConfiguration configuration, JwtService jwtService)
        {
            _db = db;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [AllowAnonymous] // Para poder ser accedido sin necesidad de token
        public async Task<IActionResult> Register(string username, string password)
        {
            try
            {
                // Revisa si el usuario ya existe
                var existingUser = await _db.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
                if (existingUser != null)
                    return BadRequest("El usuario ya existe");

                var newUser = new Usuarios
                {
                    Username = username,
                    PasswordHash = HashPassword(password) // Hashea la contraseña antes de ser guardada en la base de datos
                };

                _db.Usuarios.Add(newUser);
                await _db.SaveChangesAsync();

                return Ok("Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous] // Para poder ser accedido sin necesidad de token
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await AuthenticateUser(model.Username, model.Password);
                if (user == null)
                    return Unauthorized();

                // Generacion de token JWT
                var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        // Encuentra el usuario y verifica su contraseña con el hash guardado dentro de la base de datos
        private async Task<Usuarios> AuthenticateUser(string username, string password)
        {
            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            if (VerifyPassword(password, user.PasswordHash))
                return user;

            return null;
        }

        //  Hashea un string que sería la contraseña, ahora mismo no contiene metodo de seguridad pero se puede agregar una encriptación
        private string HashPassword(string password)
        {
            return password; 
        }

        // Compara el hash con la contraseña guardada mediante el mismo metodo de seguridad
        private bool VerifyPassword(string password, string passwordHash)
        {
            return password == passwordHash;
        }



        [HttpPut]
        [Route("UpdatePaciente")] // Hace un update a un paciente en especifico segun los datos que se le brinden. 
        [Authorize] // Esto no es realmente necesario ya que apliqué autorización globalmente. Pero lo dejo para el ejemplo.
        public async Task<Respuesta> UpdatearPaciente(int id, string? nuevoNombre, DateTime? nuevaFechaNacimiento, string? nuevoGenero, string? nuevoNumeroTelefono, string? nuevaDireccion)
        {
            Respuesta res = new Respuesta();
            try
            {
                var pac = await _db.Pacientes.FindAsync(id);
                if (pac == null)
                {
                    res.Ok = false;
                    res.Message = "No se encontró el paciente";
                    return res;
                }

                try
                {
                    if (nuevoNombre != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE Pacientes SET nombre = @Nombre WHERE id = @Id",
                            new SqlParameter("@Nombre", nuevoNombre),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (nuevaFechaNacimiento != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE Pacientes SET fecha_nacimiento = @FechaNacimiento WHERE id = @Id",
                            new SqlParameter("@FechaNacimiento", nuevaFechaNacimiento),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (nuevoGenero != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE Pacientes SET genero = @Genero WHERE id = @Id",
                            new SqlParameter("@Genero", nuevoGenero),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (nuevoNumeroTelefono != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE Pacientes SET numero_telefono = @NumeroTelefono WHERE id = @Id",
                            new SqlParameter("@NumeroTelefono", nuevoNumeroTelefono),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (nuevaDireccion != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE Pacientes SET direccion = @Direccion WHERE id = @Id",
                            new SqlParameter("@Direccion", nuevaDireccion),
                            new SqlParameter("@Id", id)
                        );
                    }

                    // Return successful response
                    res.Ok = true;
                    res.Message = "Se actualizó el paciente";
                    return res;
                }
                catch (Exception e)
                {
                    // Handle inner exception
                    res.Ok = false;
                    res.Message = "Error: " + e.Message + e.StackTrace;
                    return res;
                }
            }
            catch (Exception e)
            {
                // Handle outer exception
                res.Ok = false;
                res.Message = "Error: " + e.Message + e.StackTrace;
                return res;
            }
        }
        [HttpGet]
        [Route("GetPacientes")] // Obtiene todos los pacientes, con la posibilidad de filtrarlos si se incluye el nombre o el numero telefonico.
        [Authorize]
        public async Task<Respuesta> GetPacientes(string? nombre, string? numero_telefono)
        {
            Respuesta res = new Respuesta();
            try
            {
                IQueryable<Pacientes> query = _db.Pacientes;

                // filtra segun nombre
                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(p => p.nombre.Contains(nombre));
                }

                // filtra segun numero
                if (!string.IsNullOrEmpty(numero_telefono))
                {
                    query = query.Where(p => p.numero_telefono.Contains(numero_telefono));
                }

                var pacientes = await query.ToListAsync();

                res.Ok = true;
                res.Data = pacientes;
            }
            catch (Exception ex)
            {
                res.Ok = false;
                res.Message = ex.ToString();
            }

            return res;
        }

        [HttpGet]
        [Route("GetPaciente")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
        [Authorize]

        public async Task<Respuesta> GetPaciente(int idPaciente)
        {
            Respuesta res = new Respuesta();
            try
            {

                var Objeto = await _db.Pacientes.Where(
                t => t.id == idPaciente
                ).ToListAsync();
                res.Ok = true;
                res.Data = Objeto[0];
                return res;



            }
            catch (Exception ex)
            {
                res.Message = ex.ToString();
                res.Ok = false;
            }
            return res;
        }

        [HttpPost]
        [Route("PostNuevoPaciente")] // Crea un nuevo paciente
        [Authorize]

        public async Task<Respuesta> PostNuevoPaciente(string nombre, DateTime fechaNacimiento, string genero, string numeroTelefono, string direccion)
        {
            Respuesta res = new Respuesta();
            try
            {
                Pacientes nuevoPaciente = new Pacientes
                {
                    nombre = nombre,
                    fecha_nacimiento = fechaNacimiento,
                    genero = genero,
                    numero_telefono = numeroTelefono,
                    direccion = direccion
                };

                _db.Add(nuevoPaciente);

                await _db.SaveChangesAsync();

                res.Message = "El paciente se grabó correctamente";
                res.Ok = true;
            }
            catch (Exception ex)
            {
                res.Message = $"Error: {ex.Message}";
                res.Ok = false;
            }

            return res;
        }

        [HttpDelete]
        [Route("DeletePaciente")] // Encuentra el ID del paciente para luego eliminarlo
        [Authorize]
        public async Task<Respuesta> DeletePaciente(int idPaciente)
        {
            Respuesta res = new Respuesta();
            try
            {
                var paciente = await _db.Pacientes.FindAsync(idPaciente);

                if (paciente == null)
                {
                    res.Ok = false;
                    res.Message = $"Paciente with ID {idPaciente} not found.";
                }
                else
                {
                    _db.Pacientes.Remove(paciente);
                    await _db.SaveChangesAsync();

                    res.Ok = true;
                    res.Message = $"Paciente with ID {idPaciente} deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                res.Ok = false;
                res.Message = $"Error deleting Paciente: {ex.Message}";
            }

            return res;
        }

    }


        }

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}