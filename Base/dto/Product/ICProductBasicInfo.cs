namespace gmc_api.Base.dto.Product
{
    public class ICProductBasicInfo
    {
        public decimal ICProductWeight { get; set; }
        public decimal ICProductVolume { get; set; }
        public decimal ICProductGrossWeight { get; set; }
        public decimal ICProductNetWeight { get; set; }

        public int ICProductID { get; set; }
        public int FK_ICStkUOMID { get; set; }
    }
}
