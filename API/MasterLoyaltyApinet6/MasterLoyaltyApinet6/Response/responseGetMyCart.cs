using MasterLoyaltyApinet6.Models;

namespace apiMasterLoyalty.Response
{
    public class responseGetMyCart
    {
        public int CarritoId { get; set; }
        public int TiendaId { get; set; }
        public string Sucursal { get; set; } = null!;
        public string Articulo { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Imagen { get; set; } = null!;
        public int Cantidad { get; set; }

    }


}
