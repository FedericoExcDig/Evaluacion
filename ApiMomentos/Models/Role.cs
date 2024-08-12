using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Personal> Personals { get; } = new List<Personal>();

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
