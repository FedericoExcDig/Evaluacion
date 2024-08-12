using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Habitaciones
{
    public int HabitacionId { get; set; }

    public string? NombreHabitacion { get; set; }

    public int? CategoriaId { get; set; }

    public bool? Disponible { get; set; }

    public DateTime? ProximaReserva { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual CategoriasHabitaciones? Categoria { get; set; }

    public virtual ICollection<HabitacionesVirtuales> HabitacionesVirtualesHabitacion1s { get; } = new List<HabitacionesVirtuales>();

    public virtual ICollection<HabitacionesVirtuales> HabitacionesVirtualesHabitacion2s { get; } = new List<HabitacionesVirtuales>();

    public virtual ICollection<Movimiento> Movimientos { get; } = new List<Movimiento>();

    public virtual ICollection<Reserva> Reservas { get; } = new List<Reserva>();
}
