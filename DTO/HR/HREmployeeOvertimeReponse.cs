using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOvertimeReponse : NumberRecord
    {
        [JsonPropertyName("employeeNo")]
        public string employeeNo { get; set; }

        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }

        [JsonPropertyName("id")]
        public int HREmployeeOvertimeID { get; set; }
        [JsonPropertyName("no")]
        public string HREmployeeOvertimeNo { get; set; }
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOvertimeFromDate { get; set; }
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOvertimeToDate { get; set; }
        [JsonPropertyName("fk_overtimeRate")]
        public int FK_HROvertimeRateID { get; set; }
        [JsonPropertyName("fk_shifts")]
        public int FK_HRShiftID { get; set; }
        [JsonPropertyName("reason")]
        public string HREmployeeOvertimeReasonDetail { get; set; }
        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonPropertyName("procStepId")]
        public int FK_ADApprovalProcStepID { get; set; } = 0;
        [JsonPropertyName("inboxId")]
        public int ADInboxItemID { get; set; } = 0;
        [JsonPropertyName("CTCheck")]
        public Boolean HREmployeeOvertimeCTCheck { get; set; }
        [JsonPropertyName("breakHour")]
        public decimal HREmployeeOvertimeBreakHour { get; set; }
    }
}
