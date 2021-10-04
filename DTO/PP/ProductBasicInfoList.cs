using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class ProductBasicInfoList
    {
        [JsonPropertyName("no")]
        public string pOrdNo { get; set; }
        [JsonPropertyName("phaseName")]
        public string phaseName { get; set; }
        [JsonPropertyName("productDate")]
        public DateTime pOrdDate { get; set; }
    }
}
