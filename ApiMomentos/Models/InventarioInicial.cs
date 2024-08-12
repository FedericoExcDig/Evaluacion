using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class InventarioInicial
{
    public int ArticuloId { get; set; }

    public int? CantidadInicial { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual Articulo Articulo { get; set; } = null!;
}
