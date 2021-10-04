using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOffWorkReponse : NumberRecord
    {
        [JsonPropertyName("employeeNo")]
        public string employeeNo { get; set; }

        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }

        [JsonPropertyName("id")]
        public int HREmployeeOffWorkID { get; set; }
        [JsonPropertyName("no")]
        public string HREmployeeOffWorkNo { get; set; }
        [JsonPropertyName("fk_employeeLeaveTypes")]
        public int FK_HREmployeeLeaveTypeID { get; set; }
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOffWorkFromDate { get; set; }
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOffWorkToDate { get; set; }
        [JsonPropertyName("reason")]
        public string HREmployeeOffWorkReasonDetail { get; set; }
        [JsonPropertyName("regDays")]
        public decimal HREmployeeOffWorkRegDays { get; set; }
        [JsonPropertyName("lc_typeCombox")]
        public string HREmployeeOffWorkTypeCombo { get; set; }
        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonPropertyName("procStepId")]
        public int FK_ADApprovalProcStepID { get; set; } = 0;
        [JsonPropertyName("inboxId")]
        public int ADInboxItemID { get; set; } = 0;

    }
}
