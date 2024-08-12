using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int RolId { get; set; }

    public virtual ICollection<MovimientosUsuario> MovimientosUsuarios { get; } = new List<MovimientosUsuario>();

    public virtual Role Rol { get; set; } = null!;
}
