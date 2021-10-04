using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PO
{
    public class APPOItemInfo
    {
        [JsonPropertyName("no")]
        public string ICProductNo { get; set; }
        [JsonPropertyName("name")]
        public string ICProductName { get; set; }
        [JsonPropertyName("itemQty")]
        public decimal APPOItemQty { get; set; }
        [JsonPropertyName("units")]
        public string ICUOMName { get; set; }
        [JsonPropertyName("stkQty")]
        public decimal APPOItemStkQty { get; set; }
        [JsonPropertyName("unitPrice")]
        public decimal APPOItemUnitPrice { get; set; }
        [JsonPropertyName("summary")]
        public decimal APPOItemAmtTot { get; set; }
        [JsonPropertyName("unitStk")]
        public string ICUOMNameStk { get; set; }
        [JsonPropertyName("arrivalDate")]
        public Nullable<DateTime> APPOItemArrivalDate { get; set; }
    }
}
