using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PR
{
    public class APPRItemInfo
    {
        [JsonPropertyName("no")]
        public string ICProductNo { get; set; }
        [JsonPropertyName("name")]
        public string ICProductName { get; set; }
        [JsonPropertyName("itemQty")]
        public decimal APPRItemQty { get; set; }
        [JsonPropertyName("units")]
        public string ICUOMName { get; set; }
        [JsonPropertyName("stkQty")]
        public decimal APPRItemStkQty { get; set; }
        [JsonPropertyName("unitStk")]
        public string ICUOMNameStk { get; set; }
        [JsonPropertyName("arrivalDate")]
        public Nullable<DateTime> APPRItemArrivalDate { get; set; }
    }
}
