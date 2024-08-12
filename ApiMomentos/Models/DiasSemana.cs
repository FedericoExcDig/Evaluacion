using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class DiasSemana
{
    public int DiaSemanaId { get; set; }

    public string? NombreDiaSemana { get; set; }

    public virtual ICollection<Tarifa> Tarifas { get; } = new List<Tarifa>();
}
