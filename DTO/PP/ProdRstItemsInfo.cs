namespace gmc_api.DTO.PP
{
    public class ProdRstItemsInfo
    {
        public int itemID { get; set; }
        public string remark { get; set; }
        public string productNo { get; set; }
        public string productName { get; set; }
        public string phaseName { get; set; }
        public string unit { get; set; }

        public decimal qty { get; set; }
        public decimal cancelQty { get; set; } = 0;
        public decimal setUpQty { get; set; } = 0;
        public decimal NCRQty { get; set; } = 0;
    }
}
