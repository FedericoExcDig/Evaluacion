using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class TipoMovimiento
{
    public int TipoMovimientoId { get; set; }

    public string? NombreTipoMovimiento { get; set; }

    public string? Tipo { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; } = new List<MovimientosStock>();
}
