using gmc_api.Base.Helpers;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HREmployeeOvertimeUpdateRequest
    {
        [JsonIgnore]
        public int HREmployeeOvertimeID { get; set; }
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HREmployeeOvertimeFromDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HREmployeeOvertimeToDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("fk_overtimeRate")]
        public int FK_HROvertimeRateID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("fk_shifts")]
        public int FK_HRShiftID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("reason")]
        public string HREmployeeOvertimeReasonDetail { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("breakHour")]
        public decimal HREmployeeOvertimeBreakHour { get; set; } = Constants.DEFAULT_VALUE_DECIMAL;
        [JsonPropertyName("CTCheck")]
        public Nullable<Boolean> HREmployeeOvertimeCTCheck { get; set; } = Constants.DEFAULT_VALUE_BOOL;
        [JsonIgnore]
        public int HREmployeeOvertimePeriod { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonIgnore]
        public int HREmployeeOvertimeFiscalYear { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonIgnore]
        public Nullable<Boolean> HREmployeeOvertimeOverDayCheck { get; set; } = Constants.DEFAULT_VALUE_BOOL;
        [JsonIgnore]
        public decimal HREmployeeOvertimeHour { get; set; } = Constants.DEFAULT_VALUE_DECIMAL;
    }
}
