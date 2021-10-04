using gmc_api.Base.dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ICUOMFactors")]
    public class ICUOMFactors : FixFiveProps
    {
        [Key]
        public int ICUOMFactorID { get; set; }
        public bool AASelected { get; set; }
        public int FK_ICFromUOMID { get; set; }
        public int FK_ICToUOMID { get; set; }
        public decimal ICUOMFactorQty { get; set; }
        public string ICUOMFactorMethodCombo { get; set; }
    }
}
