using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public int? VisitaId { get; set; }

    public int? HabitacionId { get; set; }

    public DateTime? FechaReserva { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? TotalHoras { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual Habitaciones? Habitacion { get; set; }

    public virtual Visita? Visita { get; set; }
}
