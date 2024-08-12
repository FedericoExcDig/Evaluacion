using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Consumo
{
    public int ConsumoId { get; set; }

    public int? MovimientosId { get; set; }

    public int? ArticuloId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public bool? Anulado { get; set; }

    public virtual Articulo? Articulo { get; set; }

    public virtual Movimiento? Movimientos { get; set; }
}
