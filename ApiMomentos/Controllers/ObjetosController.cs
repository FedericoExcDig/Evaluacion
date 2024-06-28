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
    [Authorize] // Apply authorization globally to the controller

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
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<IActionResult> Register(string username, string password)
        {
            try
            {
                // Check if the username is already taken
                var existingUser = await _db.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
                if (existingUser != null)
                    return BadRequest("Username already exists");

                // Create a new user
                var newUser = new Usuarios
                {
                    Username = username,
                    PasswordHash = HashPassword(password) // Hash password securely before storing
                };

                _db.Usuarios.Add(newUser);
                await _db.SaveChangesAsync();

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error registering user: {ex.Message}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                // Authenticate user
                var user = await AuthenticateUser(model.Username, model.Password);
                if (user == null)
                    return Unauthorized();

                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error logging in: {ex.Message}");
            }
        }

        // Helper method to authenticate user
        private async Task<Usuarios> AuthenticateUser(string username, string password)
        {
            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            // Example: Verify password (you should hash and verify securely in real scenarios)
            if (VerifyPassword(password, user.PasswordHash))
                return user;

            return null;
        }

        // Example: Hash password (you should implement a secure hashing algorithm)
        private string HashPassword(string password)
        {
            // Example: Use a secure hashing algorithm like BCrypt or ASP.NET Core's built-in PasswordHasher
            return password; // Replace with actual hashing logic
        }

        // Example: Verify password (you should implement password verification securely)
        private bool VerifyPassword(string password, string passwordHash)
        {
            // Example: Implement secure password verification logic
            return password == passwordHash; // Replace with actual password verification logic
        }



        [HttpPut]
        [Route("UpdatePaciente")]
        [Authorize]
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
                    // Update properties based on provided values
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
        [Route("GetPacientes")]
        [Authorize]
        public async Task<Respuesta> GetPacientes(string? nombre, string? numero_telefono)
        {
            Respuesta res = new Respuesta();
            try
            {
                IQueryable<Pacientes> query = _db.Pacientes;

                // Filter by nombre if provided
                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(p => p.nombre.Contains(nombre));
                }

                // Filter by numero_telefono if provided
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
        [Route("GetPaciente")]
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
        [Route("PostNuevoPaciente")]
        [Authorize]

        public async Task<Respuesta> PostNuevoPaciente(string nombre, DateTime fechaNacimiento, string genero, string numeroTelefono, string direccion)
        {
            Respuesta res = new Respuesta();
            try
            {
                // Create a new Pacientes object and set its properties
                Pacientes nuevoPaciente = new Pacientes
                {
                    nombre = nombre,
                    fecha_nacimiento = fechaNacimiento,
                    genero = genero,
                    numero_telefono = numeroTelefono,
                    direccion = direccion
                };

                // Add the new patient to the database context
                _db.Add(nuevoPaciente);

                // Save changes to the database asynchronously
                await _db.SaveChangesAsync();

                // Prepare the response message
                res.Message = "El paciente se grabó correctamente";
                res.Ok = true;
            }
            catch (Exception ex)
            {
                // Handle exceptions and prepare error message
                res.Message = $"Error: {ex.Message}";
                res.Ok = false;
            }

            // Return the response
            return res;
        }

    }


        }

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}