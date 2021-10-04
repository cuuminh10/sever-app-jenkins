using gmc_api.Base.dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ICProductUOMs")]
    public class ICProductUOMs : FixFiveProps
    {
        [Key]
        public int ICProductUOMID { get; set; }
        public string ICProductUOMNo { get; set; }
        public string ICProductUOMName { get; set; }
        public int FK_ICProductID { get; set; }
        public string ICProductUOMDesc { get; set; }
        public decimal ICProductUOMFactor { get; set; }
        public bool ICProductUOMIsStockUnit { get; set; }
        public string ICProductUOMMethodCombo { get; set; }
        public int FK_ICUOMID { get; set; }
        public int FK_ICUOMCalculatorID { get; set; }
    }
}
