using ApiObjetos.Data;
using ApiObjetos.Models.Sistema;
using ApiObjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiObjetos.Controllers
{
    public class MovimientosController
    {
        private readonly ApplicationDbContext _db;

        public MovimientosController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<int> CrearMovimientoHabitacion(int visitaId, int totalFacturado, int habitacionId, Habitaciones habitacion)
        {
            try
            {
                Movimiento nuevoMovimiento = new Movimiento
                {
                    VisitaId = visitaId,
                    TotalFacturado = totalFacturado,
                    Habitacion = habitacion,
                    HabitacionId = habitacionId,
                };

                _db.Add(nuevoMovimiento);

                await _db.SaveChangesAsync();

                return nuevoMovimiento.MovimientosId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CrearMovimientoConsumos(int visitaId, int totalFacturado, int habitacionId, List<Consumo> consumos)
        {
            try
            {
                Movimiento nuevoMovimiento = new Movimiento
                {
                    VisitaId = visitaId,
                    TotalFacturado = totalFacturado,
                    Consumos = consumos,
                    HabitacionId = habitacionId,
                };

                _db.Add(nuevoMovimiento);

                await _db.SaveChangesAsync();

                return nuevoMovimiento.MovimientosId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        [HttpGet]
        [Route("GetMovimiento")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
        [AllowAnonymous]

        public async Task<Respuesta> GetMovimiento(int id)
        {
            Respuesta res = new Respuesta();
            try
            {

                var Objeto = await _db.Movimientos.Where(
                t => t.MovimientosId == id
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
        [Route("GetMovimientos")] // Obtiene un paciente basado en su idPaciente. Se obtiene la lista de los idPaciente con el metodo GetPacientes
        [AllowAnonymous]

        public async Task<Respuesta> GetMovimientos()
        {
            Respuesta res = new Respuesta();
            try
            {

                var Objeto = await _db.Movimientos.ToListAsync();
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


        [HttpDelete]
        [Route("AnularMovimiento")] // Encuentra el ID del paciente para luego eliminarlo
        [AllowAnonymous]
        public async Task<Respuesta> AnularMovimiento(int id, bool Estado)
        {
            Respuesta res = new Respuesta();
            try
            {
                var movimiento = await _db.Movimientos.FindAsync(id);

                if (movimiento == null)
                {
                    res.Ok = false;
                    res.Message = $"El movimiento con el id {id} no se encontró.";
                }
                else
                {
                    movimiento.Anulado = Estado;
                    await _db.SaveChangesAsync();

                    res.Ok = true;
                    res.Message = $"Se cambió el estado correctamente";
                }
            }
            catch (Exception ex)
            {
                res.Ok = false;
                res.Message = $"Ocurrió un error: {ex.Message}";
            }

            return res;
        }
    }
}
