using MasterLoyaltyApinet6.Models;

namespace apiMasterLoyalty.Response
{
    public class responseArticlesTienda
    {
        public Tiendum Tienda { get; set; } = null!;
        public List<articulosTienda> articulos { get; set; } = null!;
    }


    public class articulosTienda
    {
        public Articulo articulo { get; set; } = null!;
        public int Stock { get; set; }
        public int articuloTiendaId { get; set; }
    }
}
