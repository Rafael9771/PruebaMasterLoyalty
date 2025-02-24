using System;
using System.Collections.Generic;

namespace MasterLoyaltyApinet6.Models
{
    public partial class Carrito
    {
        public int CaId { get; set; }
        public Guid CaGuid { get; set; }
        public int CaClId { get; set; }
        public int CaArTiId { get; set; }
        public int CaCantidad { get; set; }
        public DateTime CaDcreate { get; set; }
        public byte CaStatus { get; set; }
    }
}
