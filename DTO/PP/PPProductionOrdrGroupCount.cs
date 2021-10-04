using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class PPProductionOrdrGroupCount
    {
        [JsonPropertyName("incompleted")]
        public int incompleted { get; set; } = 0;
        [JsonPropertyName("completed")]
        public int completed { get; set; } = 0;
        [JsonPropertyName("opens")]
        public int opens { get; set; } = 0;
        [JsonPropertyName("overdue")]
        public int overdue { get; set; } = 0;
    }
}
