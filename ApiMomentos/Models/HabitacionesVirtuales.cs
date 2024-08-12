using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class HabitacionesVirtuales
{
    public int HabitacionVirtualId { get; set; }

    public int? Habitacion1Id { get; set; }

    public int? Habitacion2Id { get; set; }

    public decimal? Precio { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual Habitaciones? Habitacion1 { get; set; }

    public virtual Habitaciones? Habitacion2 { get; set; }
}
