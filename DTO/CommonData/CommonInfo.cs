using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class CommonInfo
    {
        [JsonPropertyName("no")]
        public string objNo { get; set; }
        [JsonPropertyName("id")]
        public int objId { get; set; }
        [JsonPropertyName("fk_id")]
        public int foreignId { get; set; } = -1;
    }
}
