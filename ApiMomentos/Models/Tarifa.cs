using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Tarifa
{
    public int TarifaId { get; set; }

    public string? DetalleTarifa { get; set; }

    public int? CategoriaId { get; set; }

    public int? DiaSemanaId { get; set; }

    public TimeSpan? HoraInicio { get; set; }

    public TimeSpan? HoraFin { get; set; }

    public int? TipoTarifaId { get; set; }

    public decimal? Precio { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Anulado { get; set; }

    public virtual CategoriasHabitaciones? Categoria { get; set; }

    public virtual DiasSemana? DiaSemana { get; set; }

    public virtual TipoTarifa? TipoTarifa { get; set; }
}
