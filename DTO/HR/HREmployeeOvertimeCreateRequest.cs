using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOvertimeCreateRequest
    {
        [Required]
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOvertimeFromDate { get; set; }
        [Required]
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOvertimeToDate { get; set; }
        [Required]
        [JsonPropertyName("fk_overtimeRate")]
        public int FK_HROvertimeRateID { get; set; }
        [Required]
        [JsonPropertyName("fk_shifts")]
        public int FK_HRShiftID { get; set; }
        [Required]
        [JsonPropertyName("reason")]
        public string HREmployeeOvertimeReasonDetail { get; set; }
        [JsonPropertyName("breakHour")]
        public decimal HREmployeeOvertimeBreakHour { get; set; } = 0;
        [JsonPropertyName("CTCheck")]
        public Boolean HREmployeeOvertimeCTCheck { get; set; }
        [JsonIgnore]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonIgnore]
        public int FK_HREmployeeID { get; set; } = 0;
        [JsonIgnore]
        public string HREmployeeOvertimeNo { get; set; }
        [JsonIgnore]
        public int HREmployeeOvertimePeriod { get; set; }
        [JsonIgnore]
        public int HREmployeeOvertimeFiscalYear { get; set; }
        [JsonIgnore]
        public Boolean HREmployeeOvertimeOverDayCheck { get; set; } = false;
        [JsonIgnore]
        public decimal HREmployeeOvertimeHour { get; set; }
    }
}
