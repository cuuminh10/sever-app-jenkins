using gmc_api.Base.Helpers;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HRTravelCalendarUpdateRequest
    {
        [JsonIgnore]
        public int HRTravelCalendarID { get; set; }
        [JsonPropertyName("name")]
        public string HRTravelCalendarName { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("calendarDate")]
        public Nullable<DateTime> HRTravelCalendarDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("description")]
        public string HRTravelCalendarDesc { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HRTravelCalendarFromDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HRTravelCalendarToDate { get; set; } = Constants.DEFAULT_VALUE_DATETIME;
        [JsonPropertyName("fk_travelType")]
        public int FK_HRTravelTypeID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("fk_province_travelType")]
        public int FK_HRProvinceID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("sunDay")]
        public Nullable<Boolean> HRTravelCalendarSundayCheck { get; set; } = Constants.DEFAULT_VALUE_BOOL;
        [JsonPropertyName("feeCaculator")]
        public Nullable<Boolean> HRTravelCalendarNoCalCheck { get; set; } = Constants.DEFAULT_VALUE_BOOL;
    }
}
