using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class TipoTarifa
{
    public int TipoTarifaId { get; set; }

    public string? NombreTipoTarifa { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual ICollection<Tarifa> Tarifas { get; } = new List<Tarifa>();
}
