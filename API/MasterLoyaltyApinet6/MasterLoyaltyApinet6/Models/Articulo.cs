﻿using System;
using System.Collections.Generic;

namespace MasterLoyaltyApinet6.Models
{
    public partial class Articulo
    {
        public int ArId { get; set; }
        public Guid ArGuid { get; set; }
        public string ArNombre { get; set; } = null!;
        public string ArCodigo { get; set; } = null!;
        public string ArDescripcion { get; set; } = null!;
        public decimal ArPrecio { get; set; }
        public string ArImagen { get; set; } = null!;
        public DateTime ArDcreate { get; set; }
        public byte ArStatus { get; set; }
    }
}
