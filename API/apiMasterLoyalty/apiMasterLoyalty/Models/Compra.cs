using System;
using System.Collections.Generic;

namespace apiMasterLoyalty.Models;

public partial class Compra
{
    public int CoId { get; set; }

    public Guid CoGuid { get; set; }

    public int CoClId { get; set; }

    public int CoTiId { get; set; }

    public int CoArId { get; set; }

    public string CoFolio { get; set; } = null!;

    public int CoCantidad { get; set; }

    public DateTime CoDcreate { get; set; }

    public byte CoStatus { get; set; }
}
