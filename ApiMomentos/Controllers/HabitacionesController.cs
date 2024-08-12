using ApiObjetos.Models.Sistema;
using ApiObjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ApiObjetos.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiObjetos.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HabitacionesController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
        }
            #region Habitaciones
            [HttpPost]
            [Route("CrearHabitacion")] // Crea un nuevo paciente
            [AllowAnonymous]

            public async Task<Respuesta> CrearHabitacion(string nombreHabitacion, int categoriaID)
            {
                Respuesta res = new Respuesta();
                try
                {
                    Habitaciones nuevaHabitacion = new Habitaciones
                    {

                        NombreHabitacion = nombreHabitacion,
                        CategoriaId = categoriaID,
                    };

                    _db.Add(nuevaHabitacion);
                    await _db.SaveChangesAsync();

                    res.Message = "La habitación se creó correctamente";
                    res.Ok = true;
                }
                catch (Exception ex)
                {
                    res.Message = $"Error: {ex.Message}";
                    res.Ok = false;
                }

                return res;
            }


            [HttpGet]
            [Route("GetHabitacion")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
            [AllowAnonymous]

            public async Task<Respuesta> GetHabitacion(int idHabitacion)
            {
                Respuesta res = new Respuesta();
                try
                {

                    var Objeto = await _db.Habitaciones.Where(
                    t => t.HabitacionId == idHabitacion
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

            [HttpGet]
            [Route("GetHabitaciones")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
            [AllowAnonymous]

            public async Task<Respuesta> GetHabitaciones()
            {
                Respuesta res = new Respuesta();
                try
                {

                    var Objeto = await _db.Habitaciones.ToListAsync();
                    res.Ok = true;
                    res.Data = Objeto;
                    return res;



                }
                    catch (Exception ex)
                {
                    res.Message = "Error " + ex.ToString();
                    res.Ok = false;
                }
                return res;
            }

            [HttpPut]
            [Route("ActualizarHabitacion")] // Hace un update a un paciente en especifico segun los datos que se le brinden. 
            [AllowAnonymous]
            public async Task<Respuesta> ActualizarHabitacion(int id, string? nuevoNombre, int nuevaCategoria, string? disponibilidad, DateTime proximaReserva, int usuarioId)
            {
                Respuesta res = new Respuesta();
                try
                {
                    var pac = await _db.Habitaciones.FindAsync(id);
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
                                "UPDATE Habitaciones SET NombreHabitacion = @Nombre WHERE HabitacionID = @Id",
                                new SqlParameter("@Nombre", nuevoNombre),
                                new SqlParameter("@Id", id)
                            );
                        }
                        if (nuevaCategoria != null)
                        {
                            _db.Database.ExecuteSqlRaw(
                                "UPDATE Habitaciones SET CategoriaID = @CategoriaID WHERE HabitacionID = @Id",
                                new SqlParameter("@CategoriaID", nuevaCategoria),
                                new SqlParameter("@Id", id)
                            );
                        }
                        if (disponibilidad != null)
                        {
                            _db.Database.ExecuteSqlRaw(
                                "UPDATE Habitaciones SET Disponible = @Disponibilidad WHERE HabitacionID = @Id",
                                new SqlParameter("@Disponibilidad", disponibilidad),
                                new SqlParameter("@Id", id)
                            );
                        }
                        if (proximaReserva != null)
                        {
                            _db.Database.ExecuteSqlRaw(
                                "UPDATE Habitaciones SET ProximaReserva = @ProximaReserva WHERE HabitacionID = @Id",
                                new SqlParameter("@ProximaReserva", proximaReserva),
                                new SqlParameter("@Id", id)
                            );
                        }
                        if (usuarioId != null)
                        {
                            _db.Database.ExecuteSqlRaw(
                                "UPDATE Habitaciones SET UsuarioID = @UsuarioID WHERE HabitacionID = @Id",
                                new SqlParameter("@UsuarioID", usuarioId),
                                new SqlParameter("@Id", id)
                            );
                        }

                        // Return successful response
                        res.Ok = true;
                        res.Message = "Se actualizó la habitación";
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

            [HttpDelete]
            [Route("AnularHabitacion")] // Encuentra el ID del paciente para luego eliminarlo
            [AllowAnonymous]
            public async Task<Respuesta> AnularHabitacion(int idHabitacion, bool Estado)
            {
                Respuesta res = new Respuesta();
                try
                {
                    var habitacion = await _db.Habitaciones.FindAsync(idHabitacion);

                    if (habitacion == null)
                    {
                        res.Ok = false;
                        res.Message = $"La habitación con el id {idHabitacion} no se encontró.";
                    }
                    else
                    {
                        habitacion.Anulado = Estado;
                        await _db.SaveChangesAsync();

                        res.Ok = true;
                        res.Message = $"Se anuló la habitación correctamente";
                    }
                }
                catch (Exception ex)
                {
                    res.Ok = false;
                    res.Message = $"Ocurrió un error: {ex.Message}";
                }

                return res;
            }
            #endregion

        }
    }

