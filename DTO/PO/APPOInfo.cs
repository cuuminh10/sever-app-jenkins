using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PO
{
    public class APPOInfo : NumberRecord
    {
        [JsonPropertyName("employeeNo")]
        public string employeeNo { get; set; }

        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }
        [JsonPropertyName("nccName")]
        public string nccName { get; set; }

        [JsonPropertyName("id")]
        public int APPOID { get; set; }
        [JsonPropertyName("no")]
        public string APPONo { get; set; }
        [JsonPropertyName("apprDate")]
        public Nullable<DateTime> APPODate { get; set; }
        [JsonPropertyName("description")]
        public string APPODesc { get; set; }
        [JsonPropertyName("summary")]
        public decimal APPOAmtTot { get; set; }

        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonPropertyName("procStepId")]
        public int FK_ADApprovalProcStepID { get; set; } = 0;
        [JsonPropertyName("inboxId")]
        public int ADInboxItemID { get; set; } = 0;
        public int displayReject { get; set; } = 0;
    }
}
