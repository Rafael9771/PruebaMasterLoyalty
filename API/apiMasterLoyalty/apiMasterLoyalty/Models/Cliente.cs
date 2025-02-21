using System;
using System.Collections.Generic;

namespace apiMasterLoyalty.Models;

public partial class Cliente
{
    public int CiId { get; set; }

    public Guid CiGuid { get; set; }

    public string CiNombre { get; set; } = null!;

    public string CiPrimerApellido { get; set; } = null!;

    public string CiSegundoApellido { get; set; } = null!;

    public string CiPassword { get; set; } = null!;

    public string CiCorreo { get; set; } = null!;

    public DateTime CiDcreate { get; set; }

    public byte CiStatus { get; set; }
}
