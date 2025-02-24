namespace apiMasterLoyalty.Request
{
    public class createArticuloReq
    {
        public string Nombre { get; set; } = null!;

        public string Codigo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public decimal Precio { get; set; }

        public string Imagen { get; set; } = null!;

    }
}
