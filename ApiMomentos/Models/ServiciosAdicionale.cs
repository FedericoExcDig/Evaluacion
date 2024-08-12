using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class ServiciosAdicionale
{
    public int ServicioId { get; set; }

    public string? NombreServicio { get; set; }

    public decimal? Precio { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<MovimientosServicio> MovimientosServicios { get; } = new List<MovimientosServicio>();
}
