using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class MediosPago
{
    public int MedioPagoId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Pago> Pagos { get; } = new List<Pago>();
}
