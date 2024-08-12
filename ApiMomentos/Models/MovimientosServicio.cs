using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class MovimientosServicio
{
    public int MovimientosServicioId { get; set; }

    public int? MovimientosId { get; set; }

    public int? ServicioId { get; set; }

    public int? Cantidad { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual Movimiento? Movimientos { get; set; }

    public virtual ServiciosAdicionale? Servicio { get; set; }
}
