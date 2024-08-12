using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Visita
{
    public int VisitaId { get; set; }

    public string? PatenteVehiculo { get; set; }

    public string? NumeroTelefono { get; set; }

    public DateTime? FechaPrimerIngreso { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<Movimiento> Movimientos { get; } = new List<Movimiento>();

    public virtual ICollection<Reserva> Reservas { get; } = new List<Reserva>();
}
