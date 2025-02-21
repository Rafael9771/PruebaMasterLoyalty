using System;
using System.Collections.Generic;

namespace apiMasterLoyalty.Models;

public partial class TiendaArticulo
{
    public int TiarId { get; set; }

    public Guid TiarGuid { get; set; }

    public int TiarTiId { get; set; }

    public int TiarArId { get; set; }

    public DateTime TiarDcreate { get; set; }

    public byte TiarStatus { get; set; }
}
