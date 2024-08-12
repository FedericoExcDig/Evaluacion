using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Movimiento
{
    public int MovimientosId { get; set; }

    public int? VisitaId { get; set; }
    public int? PagoId { get; set; }

    public int? HabitacionId { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? TotalHoras { get; set; }

    public decimal? TotalFacturado { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<Consumo> Consumos { get; set; } = new List<Consumo>();

    public virtual Habitaciones? Habitacion { get; set; }

    public virtual ICollection<MovimientosServicio> MovimientosServicios { get; } = new List<MovimientosServicio>();

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; } = new List<MovimientosStock>();


    public virtual Visita? Visita { get; set; }

    public virtual Pago? Pago { get; set; } // Navigation property to Pago

}
