using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class MovimientosUsuario
{
    public int MovimientoId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaHora { get; set; }

    public string Accion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
