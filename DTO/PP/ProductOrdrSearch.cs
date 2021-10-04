using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class ProductOrdrSearch
    {
        [JsonPropertyName("workOrderId")]
        public int workOrderId { get; set; }
        [JsonPropertyName("productStartDate")]
        public DateTime productStartDate { get; set; }
        [JsonPropertyName("productEndDate")]
        public DateTime productEndDate { get; set; }
        [JsonPropertyName("phaseID")]
        public int phaseID { get; set; }
    }
}
