using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Personal
{
    public int PersonalId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public int RolId { get; set; }

    public virtual Role Rol { get; set; } = null!;
}
