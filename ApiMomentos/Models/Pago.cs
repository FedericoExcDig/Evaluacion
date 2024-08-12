using System;
using System.Collections.Generic;

namespace ApiObjetos.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int? OrigenId { get; set; }

    public string? TipoOrigen { get; set; }

    public decimal? Monto { get; set; }

    public int? MedioPagoId { get; set; }

    public virtual MediosPago? MedioPago { get; set; }

    public virtual Movimiento? Origen { get; set; }

    public virtual Reserva? OrigenNavigation { get; set; }

    public virtual ICollection<Movimiento> Movimientos { get; } = new List<Movimiento>();

}
