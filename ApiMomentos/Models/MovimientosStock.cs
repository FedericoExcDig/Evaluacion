using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class MovimientosStock
{
    public int MovimientoId { get; set; }

    public int? ArticuloId { get; set; }

    public int? TipoMovimientoId { get; set; }

    public int? Cantidad { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public int? MovimientosId { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual Articulo? Articulo { get; set; }

    public virtual Movimiento? Movimientos { get; set; }

    public virtual TipoMovimiento? TipoMovimiento { get; set; }
}
