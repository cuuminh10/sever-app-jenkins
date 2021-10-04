using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOffWorkCreateRequest
    {
        [JsonIgnore]
        public int FK_HREmployeeID { get; set; } = 0;
        [Required]
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOffWorkFromDate { get; set; }
        [Required]
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOffWorkToDate { get; set; }
        [Required]
        [JsonPropertyName("lc_typeCombox")]
        [RegularExpression("UnPermision|Permision")]
        public string HREmployeeOffWorkTypeCombo { get; set; }
        [Required]
        [JsonPropertyName("reason")]
        public string HREmployeeOffWorkReasonDetail { get; set; }
        [Required]
        [JsonPropertyName("fk_employeeLeaveTypes")]
        public int FK_HREmployeeLeaveTypeID { get; set; }
        [Required]
        [JsonPropertyName("regDays")]
        public decimal HREmployeeOffWorkRegDays { get; set; }
        [JsonIgnore]
        public string HREmployeeOffWorkStatus { get; set; } = "New";
        [JsonIgnore]
        public int HREmployeeOffWorkPeriod { get; set; }
        [JsonIgnore]
        public int HREmployeeOffWorkFiscalYear { get; set; }
        [JsonIgnore]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonIgnore]
        public string HREmployeeOffWorkNo { get; set; }
        [JsonIgnore]
        public int FK_HRPositionID { get; set; } = 0;
        [JsonIgnore]
        public int FK_HRSectionID { get; set; } = 0;
    }
}
