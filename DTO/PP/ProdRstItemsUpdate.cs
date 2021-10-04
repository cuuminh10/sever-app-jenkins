using gmc_api.Base.Helpers;

namespace gmc_api.DTO.PP
{
    public class ProdRstItemsUpdate
    {
        public int id { get; set; }
        public decimal qty { get; set; } = Constants.DEFAULT_VALUE_INT;
        public decimal cancelQty { get; set; } = Constants.DEFAULT_VALUE_INT;
        public decimal setUpQty { get; set; } = Constants.DEFAULT_VALUE_INT;
        public decimal NCRQty { get; set; } = Constants.DEFAULT_VALUE_INT;
    }
}
