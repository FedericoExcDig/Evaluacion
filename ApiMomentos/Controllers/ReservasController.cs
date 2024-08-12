using ApiObjetos.Models.Sistema;
using ApiObjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ApiObjetos.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiObjetos.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly MovimientosController _movimiento;

        public ReservasController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _movimiento = new MovimientosController(db);

        }
        #region Reservas
        [HttpPost]
        [Route("ReservarHabitacion")]
        [AllowAnonymous]
        public async Task<Respuesta> ReservarHabitacion(int VisitaID, int HabitacionID, DateTime FechaReserva, DateTime FechaFin, int TotalHoras, int UsuarioID)
        {
            Respuesta res = new Respuesta();
            try
            {
                var habitacion = await GetHabitacionById(HabitacionID);
                if (habitacion == null)
                {
                    res.Message = "Habitación no encontrada.";
                    res.Ok = false;
                    return res;
                }

                Reserva nuevaReserva = new Reserva
                {
                    VisitaId = VisitaID,
                    HabitacionId = HabitacionID,
                    FechaReserva = FechaReserva,
                    FechaFin = FechaFin,
                    TotalHoras = TotalHoras,
                    UsuarioId = UsuarioID,
                    FechaRegistro = DateTime.Now,
                    Anulado = false,
                    Habitacion = habitacion
                };

                _db.Add(nuevaReserva);
                await _movimiento.CrearMovimientoHabitacion(VisitaID, (int)habitacion.Categoria.PrecioNormal, HabitacionID, habitacion);
                await _db.SaveChangesAsync();

                res.Message = "La reserva se grabó correctamente";
                res.Ok = true;
            }
            catch (Exception ex)
            {
                res.Message = $"Error: {ex.Message} {ex.InnerException}";
                res.Ok = false;
            }

            return res;
        }

        private async Task<Habitaciones?> GetHabitacionById(int habitacionId)
        {
                var a = await _db.Habitaciones
                .Include(h => h.Categoria) // Include Categoria if needed
                .FirstOrDefaultAsync(h => h.HabitacionId == habitacionId);

            return a;
        }

        public async Task<decimal?> ObtenerPrecioNormal(int reservaId)
        {
            // Fetch the Reserva along with related Habitacion and Categoria
            var reserva = await _db.Reservas
                .Include(r => r.Habitacion) // Load related Habitacion
                .ThenInclude(h => h.Categoria) // Load related Categoria
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId);

            // Check if the reserva, Habitacion, or Categoria is null
            if (reserva == null || reserva.Habitacion == null || reserva.Habitacion.Categoria == null)
            {
                return null; // Or handle the null case as needed
            }

            // Return the PrecioNormal from Categoria
            return reserva.Habitacion.Categoria.PrecioNormal;
        }

        [HttpGet]
        [Route("GetHorariosLibres")]
        [AllowAnonymous]
        public async Task<Respuesta> GetHorariosLibres(int idHabitacion, DateTime dia)
        {
            Respuesta res = new Respuesta();
            try
            {
                var horariosLibres = await HorariosLibres(idHabitacion, dia);

                res.Message = "Este es/son los rangos de horario libres";
                res.Ok = true;
                res.Data = horariosLibres;
            }
            catch (Exception ex)
            {
                res.Message = $"Error: {ex.Message}";
                res.Ok = false;
            }

            return res;
        }


        private async Task<List<TimeRange>> HorariosLibres(int idHabitacion, DateTime dia)
        {
            // Set start and end time based on the specific date
            TimeSpan dayStart = new TimeSpan(0, 0, 0); // Start at 00:00 on the given day
            TimeSpan dayEnd = new TimeSpan(23, 59, 59); // End at 23:59 on the given day
            TimeSpan minGap = TimeSpan.FromMinutes(30);

            // Fetch reservations that might affect the availability on the given day
            var reservas = await _db.Reservas
                                    .Where(r => r.HabitacionId == idHabitacion &&
                                                r.FechaReserva.Value.Date <= dia.Date &&
                                                r.FechaFin.Value.Date >= dia.Date)
                                    .Select(r => new {
                                        FechaReserva = r.FechaReserva.Value,
                                        FechaFin = r.FechaFin.Value
                                    })
                                    .OrderBy(r => r.FechaReserva)
                                    .ToListAsync();

                List<TimeRange> availablePeriods = new List<TimeRange>();

            if (!reservas.Any())
            {
                // If no reservations, the whole day is free
                availablePeriods.Add(new TimeRange { Start = dayStart, End = dayEnd });
                return availablePeriods;
            }

            // Adjust reservations that span across the given day
            var adjustedReservas = new List<(TimeSpan Start, TimeSpan End)>();
            foreach (var reserva in reservas)
            {
                var reservaStart = reserva.FechaReserva.TimeOfDay;
                var reservaEnd = reserva.FechaFin.TimeOfDay;

                if ((reserva.FechaFin > reserva.FechaReserva.Date ) && (reserva.FechaFin.Date > reserva.FechaReserva.Date))
                {
                    // Reservation spans across days, adjust times for the specific day
                    if (reserva.FechaReserva.Date == dia.Date)
                    {
                        adjustedReservas.Add((reservaStart, dayEnd));
                    }
                    else if (reserva.FechaFin.Date == dia.Date)
                    {
                        adjustedReservas.Add((dayStart, reservaEnd));
                    }
                }
                else
                {
                    adjustedReservas.Add((reservaStart, reservaEnd));
                }
            }

            // Determine free time ranges on the specific day
            TimeSpan lastEnd = dayStart;
            foreach (var (reservaStart, reservaEnd) in adjustedReservas.OrderBy(r => r.Start))
            {
                if (reservaStart - lastEnd >= minGap)
                {
                    availablePeriods.Add(new TimeRange { Start = lastEnd, End = reservaStart });
                }
                lastEnd = reservaEnd > lastEnd ? reservaEnd : lastEnd;
            }

            // Check the time after the last reservation
            if (dayEnd - lastEnd >= minGap)
            {
                availablePeriods.Add(new TimeRange { Start = lastEnd, End = dayEnd });
            }

            return availablePeriods;
        }
        #endregion

    }
}

public class TimeRange
{
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}