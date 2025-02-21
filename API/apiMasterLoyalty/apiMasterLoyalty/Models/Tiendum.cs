using System;
using System.Collections.Generic;

namespace apiMasterLoyalty.Models;

public partial class Tiendum
{
    public int TiId { get; set; }

    public Guid TiGuid { get; set; }

    public string TiSucursal { get; set; } = null!;

    public string TiDireccion { get; set; } = null!;

    public DateTime TiDcreate { get; set; }

    public byte TiStatus { get; set; }
}
