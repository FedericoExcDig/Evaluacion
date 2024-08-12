using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Articulo
{
    public int ArticuloId { get; set; }

    public string? NombreArticulo { get; set; }

    public decimal? Precio { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<Consumo> Consumos { get; } = new List<Consumo>();

    public virtual InventarioInicial? InventarioInicial { get; set; }

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; } = new List<MovimientosStock>();
}
