using gmc_api.Base.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class ProdRstUpdateRequest
    {
        public string description { get; set; } = Constants.DEFAULT_VALUE_STRING;
        //public DateTime ordDate { get; set; }

        [JsonPropertyName("detail")]
        public List<ProdRstItemsUpdate> detail { get; set; }

    }
}
