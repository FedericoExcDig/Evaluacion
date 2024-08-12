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
using Newtonsoft.Json.Linq;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ApiObjetos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Aplica la seguridad a todos los metodos globalmente

    public class ObjetosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public ObjetosController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

   
        #region Categorias

        [HttpPost]
        [Route("CrearCategoria")] // Crea un nuevo paciente
        [AllowAnonymous]

        public async Task<Respuesta> CrearCategoria(string nombreCategoria, int Precio, int? capacidadMaxima, int? UsuarioID)
        {
            Respuesta res = new Respuesta();
            try
            {
                CategoriasHabitaciones nuevaCategoria = new CategoriasHabitaciones
                {

                    NombreCategoria = nombreCategoria,
                    PrecioNormal = Precio,
                    FechaRegistro = DateTime.Now
                };

                _db.Add(nuevaCategoria);

                await _db.SaveChangesAsync();

                res.Message = "La categoría se creó correctamente";
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
        [Route("GetCategoria")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
        [AllowAnonymous]

        public async Task<Respuesta> GetCategoria(int id)
        {
            Respuesta res = new Respuesta();
            try
            {

                var Objeto = await _db.CategoriasHabitaciones.Where(
                t => t.CategoriaId == id
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
        [Route("GetCategorias")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
        [AllowAnonymous]

        public async Task<Respuesta> GetCategorias()
        {
            Respuesta res = new Respuesta();
            try
            {

                var Objeto = await _db.CategoriasHabitaciones.ToListAsync();
                res.Ok = true;
                res.Data = Objeto;
                return res;



            }   
            catch (Exception ex)
            {
                res.Message = ex.ToString();
                res.Ok = false;
            }
            return res;
        }

        [HttpPut]
        [Route("ActualizarCategoria")] // Hace un update a un paciente en especifico segun los datos que se le brinden. 
        [AllowAnonymous]
        public async Task<Respuesta> ActualizarCategoria(int id, string? nuevoNombre, int nuevaCapacidad, int Precio)
        {
            Respuesta res = new Respuesta();
            try
            {
                var pac = await _db.CategoriasHabitaciones.FindAsync(id);
                if (pac == null)
                {
                    res.Ok = false;
                    res.Message = "No se encontró la categoría";
                    return res;
                }

                try
                {
                    if (nuevoNombre != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE CategoriasHabitaciones SET NombreCategoria = @Nombre WHERE CategoriaID = @Id",
                            new SqlParameter("@Nombre", nuevoNombre),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (nuevaCapacidad != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE CategoriasHabitaciones SET CapacidadMaxima = @nuevaCapacidad WHERE CategoriaID = @Id",
                            new SqlParameter("@nuevaCapacidad", nuevaCapacidad),
                            new SqlParameter("@Id", id)
                        );
                    }
                    if (Precio != null)
                    {
                        _db.Database.ExecuteSqlRaw(
                            "UPDATE CategoriasHabitaciones SET PrecioNormal = @Precio WHERE CategoriaID = @Id",
                            new SqlParameter("@Precio", Precio),
                            new SqlParameter("@Id", id)
                        );
                    }

                    // Return successful response
                    res.Ok = true;
                    res.Message = "Se actualizó la categoría";
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
        [Route("AnularCategoria")] // Encuentra el ID del paciente para luego eliminarlo
        [AllowAnonymous]
        public async Task<Respuesta> AnularCategoria(int id, bool Estado)
        {
            Respuesta res = new Respuesta();
            try
            {
                var habitacion = await _db.CategoriasHabitaciones.FindAsync(id);

                if (habitacion == null)
                {
                    res.Ok = false;
                    res.Message = $"La habitación con el id {id} no se encontró.";
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
        [HttpPut]
        [Route("Update/{modelName}/{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Update(string modelName, int id, [FromBody] JObject data)
        {
            try
            {
                // Get the type of the model from its name
                Type modelType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == modelName);
                if (modelType == null)
                {
                    return BadRequest($"Model '{modelName}' not found.");
                }

                // Get the DbSet for the model type using reflection
                var dbSet = typeof(DbContext).GetMethod("Set", new Type[] { }).MakeGenericMethod(modelType).Invoke(_db, null);
                if (dbSet == null)
                {
                    return BadRequest("Invalid model type.");
                }

                // Find the entity by ID using reflection
                var findMethod = dbSet.GetType().GetMethod("FindAsync", new Type[] { typeof(object[]) });
                var entityTask = (Task)findMethod.Invoke(dbSet, new object[] { new object[] { id } });
                await entityTask.ConfigureAwait(false);
                var entity = ((dynamic)entityTask).Result;
                if (entity == null)
                {
                    return NotFound(new { Message = $"{modelName} with ID {id} not found.", Ok = false });
                }

                // Update the entity with new data
                var updatedEntity = data.ToObject(modelType);
                foreach (var property in modelType.GetProperties())
                {
                    var newValue = property.GetValue(updatedEntity);
                    if (newValue != null)
                    {
                        property.SetValue(entity, newValue);
                    }
                }

                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return Ok(new { Message = $"{modelName} with ID {id} updated successfully.", Ok = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error updating {modelName}: {ex.Message}", Ok = false });
            }
        }
        [HttpGet]
        [Route("Get/{modelName}/{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Get(string modelName, int id)
        {
            try
            {
                // Get the type of the model from its name
                Type modelType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == modelName);
                if (modelType == null)
                {
                    return BadRequest($"Model '{modelName}' not found.");
                }

                // Get the DbSet for the model type using reflection
                var dbSet = typeof(DbContext).GetMethod("Set", new Type[] { }).MakeGenericMethod(modelType).Invoke(_db, null);
                if (dbSet == null)
                {
                    return BadRequest("Invalid model type.");
                }

                // Find the entity by ID using reflection
                var findMethod = dbSet.GetType().GetMethod("FindAsync", new Type[] { typeof(object[]) });
                var entityTask = (Task)findMethod.Invoke(dbSet, new object[] { new object[] { id } });
                await entityTask.ConfigureAwait(false);
                var entity = ((dynamic)entityTask).Result;
                if (entity == null)
                {
                    return NotFound(new { Message = $"{modelName} with ID {id} not found.", Ok = false });
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error retrieving {modelName}: {ex.Message}", Ok = false });
            }
        }


        [HttpPost]
        [Route("Create/{modelName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string modelName, [FromBody] JsonElement data)
        {
            if (data.ValueKind == JsonValueKind.Undefined || data.ValueKind == JsonValueKind.Null)
            {
                return BadRequest("The data field is required.");
            }

            try
            {
                // Get the type of the model from its name
                Type modelType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == modelName);
                if (modelType == null)
                {
                    return BadRequest($"Model '{modelName}' not found.");
                }

                // Create an instance of the model
                var modelInstance = Activator.CreateInstance(modelType);

                // Populate the model with data
                foreach (var property in data.EnumerateObject())
                {
                    var propertyInfo = modelType.GetProperty(property.Name);
                    if (propertyInfo != null)
                    {
                        var value = ConvertJsonElement(property.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(modelInstance, value);
                    }
                }

                // Get the DbSet for the model type using reflection
                var dbSetMethod = typeof(DbContext).GetMethod("Set", Type.EmptyTypes);
                var dbSet = dbSetMethod?.MakeGenericMethod(modelType).Invoke(_db, null);
                if (dbSet == null)
                {
                    return BadRequest("Invalid model type.");
                }

                // Add the model to the DbSet
                var addMethod = dbSet.GetType().GetMethod("Add");
                addMethod?.Invoke(dbSet, new[] { modelInstance });

                await _db.SaveChangesAsync();

                return Ok(new { Message = $"{modelName} created successfully.", Ok = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error creating {modelName}: {ex.Message} + {ex.InnerException}", Ok = false });
            }
        }

        [HttpPost]
        [Route("ReservarHabitacion")] // Crea un nuevo paciente

        private object ConvertJsonElement(JsonElement jsonElement, Type targetType)
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Get the underlying type
                var underlyingType = Nullable.GetUnderlyingType(targetType);
                if (jsonElement.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }
                return ConvertJsonElement(jsonElement, underlyingType);
            }

            // Handle non-nullable types
            if (targetType == typeof(string))
            {
                return jsonElement.GetString();
            }
            if (targetType == typeof(int))
            {
                return jsonElement.GetInt32();
            }
            if (targetType == typeof(bool))
            {
                return jsonElement.GetBoolean();
            }
            if (targetType == typeof(DateTime))
            {
                return jsonElement.GetDateTime();
            }
            if (targetType == typeof(decimal))
            {
                return jsonElement.GetDecimal();
            }
            if (targetType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(jsonElement.GetString());
            }

            // Handle other types as needed
            throw new NotSupportedException($"The type '{targetType.FullName}' is not supported.");
        }

        [HttpDelete]
        [Route("Delete/{modelName}/{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Delete(string modelName, int id)
        {
            try
            {
                // Get the type of the model from its name
                Type modelType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == modelName);
                if (modelType == null)
                {
                    return BadRequest($"Model '{modelName}' not found.");
                }

                // Get the DbSet for the model type using reflection
                var dbSet = typeof(DbContext).GetMethod("Set", new Type[] { }).MakeGenericMethod(modelType).Invoke(_db, null);
                if (dbSet == null)
                {
                    return BadRequest("Invalid model type.");
                }

                // Find the entity by ID using reflection
                var findMethod = dbSet.GetType().GetMethod("FindAsync", new Type[] { typeof(object[]) });
                var entityTask = (Task)findMethod.Invoke(dbSet, new object[] { new object[] { id } });
                await entityTask.ConfigureAwait(false);
                var entity = ((dynamic)entityTask).Result;
                if (entity == null)
                {
                    return NotFound(new { Message = $"{modelName} with ID {id} not found.", Ok = false });
                }

                // Remove the entity
                var removeMethod = dbSet.GetType().GetMethod("Remove");
                removeMethod.Invoke(dbSet, new object[] { entity });

                await _db.SaveChangesAsync();

                return Ok(new { Message = $"{modelName} with ID {id} deleted successfully.", Ok = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error deleting {modelName}: {ex.Message}", Ok = false });
            }
        }

    }


        }

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}