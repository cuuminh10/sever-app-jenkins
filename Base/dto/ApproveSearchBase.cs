using System;
using System.Text.Json.Serialization;

namespace gmc_api.Base.dto
{
    public class ApproveSearchBase : Paging
    {
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> fromDate { get; set; } = null;
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> toDate { get; set; } = null;
        [JsonPropertyName("isApprove")]
        public int isApprove { get; set; } = 0;
        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; }
        [JsonPropertyName("id")]
        public int id { get; set; } = 0;
    }
}
