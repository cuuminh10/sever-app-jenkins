using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class ProdRstCreateRequest
    {
        public string description { get; set; }
        //public DateTime ordDate { get; set; }

        [JsonPropertyName("detail")]
        public List<ProdRstItemsInfo> detail { get; set; }

    }
}
