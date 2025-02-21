using System;
using System.Collections.Generic;

namespace apiMasterLoyalty.Models;

public partial class Carrito
{
    public int CaId { get; set; }

    public Guid CaGuid { get; set; }

    public int CaClId { get; set; }

    public int CaTiId { get; set; }

    public int CaArId { get; set; }

    public int CaCantidad { get; set; }

    public DateTime CaDcreate { get; set; }

    public byte CaStatus { get; set; }
}
