using System;
using gmc_api.Base.Helpers;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOffWorkUpdateRequest
    {
        [JsonIgnore]
        public int HREmployeeOffWorkID { get; set; }
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOffWorkFromDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOffWorkToDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("lc_typeCombox")] //UnPermision - Permision
                                            //  [RegularExpression("UnPermision|Permision")]
        public string HREmployeeOffWorkTypeCombo { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("reason")]
        public string HREmployeeOffWorkReasonDetail { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("fk_employeeLeaveTypes")]
        public int FK_HREmployeeLeaveTypeID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("regDays")]
        public decimal HREmployeeOffWorkRegDays { get; set; } = Constants.DEFAULT_VALUE_DECIMAL;
        [JsonIgnore]
        public int HREmployeeOffWorkPeriod { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonIgnore]
        public int HREmployeeOffWorkFiscalYear { get; set; } = Constants.DEFAULT_VALUE_INT;
    }
}
