using System.Text.Json.Serialization;

namespace gmc_api.DTO.AR
{
    public class ARSOItemInfo
    {
        [JsonPropertyName("no")]
        public string ICProductNo { get; set; }
        [JsonPropertyName("name")]
        public string ICProductName { get; set; }
        [JsonPropertyName("itemQty")]
        public decimal ARSOItemQty { get; set; }
        [JsonPropertyName("units")]
        public string ICUOMName { get; set; }
        [JsonPropertyName("unitPriceF")]
        public decimal ARSOItemFUnitPrice { get; set; }
        [JsonPropertyName("priceF")]
        public decimal ARSOItemFPrice { get; set; }
        [JsonPropertyName("taxPriceF")]
        public decimal ARSOItemFTxAmt { get; set; }
        [JsonPropertyName("summaryF")]
        public decimal ARSOItemFAmtTot { get; set; }
        [JsonPropertyName("unitPrice")]
        public decimal ARSOItemUnitPrice { get; set; }
        [JsonPropertyName("price")]
        public decimal ARSOItemPrice { get; set; }
        [JsonPropertyName("taxPrice")]
        public decimal ARSOItemTxAmt { get; set; }
        [JsonPropertyName("summary")]
        public decimal ARSOItemAmtTot { get; set; }
    }
}
