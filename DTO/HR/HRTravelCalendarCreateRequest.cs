using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HRTravelCalendarCreateRequest
    {
        [JsonPropertyName("name")]
        public string HRTravelCalendarName { get; set; }
        [JsonPropertyName("calendarDate")]
        public Nullable<DateTime> HRTravelCalendarDate { get; set; } = DateTime.Now;
        [JsonPropertyName("description")]
        public string HRTravelCalendarDesc { get; set; }
        [Required]
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HRTravelCalendarFromDate { get; set; }
        [Required]
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HRTravelCalendarToDate { get; set; }
        [Required]
        [JsonPropertyName("fk_travelType")]
        public int FK_HRTravelTypeID { get; set; }
        [Required]
        [JsonPropertyName("fk_province_travelType")]
        public int FK_HRProvinceID { get; set; }
        [JsonPropertyName("sunDay")]
        public bool HRTravelCalendarSundayCheck { get; set; }
        [JsonPropertyName("feeCaculator")]
        public bool HRTravelCalendarNoCalCheck { get; set; }
        [JsonIgnore]
        public int FK_HRDepartmentID { get; set; }
        [JsonIgnore]
        public string HRTravelCalendarNo { get; set; }
        [JsonIgnore]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonIgnore]
        public int HRTravelCalendarTempDay { get; set; }
        [JsonIgnore]
        public int HRTravelCalendarRealDay { get; set; }
    }
}
