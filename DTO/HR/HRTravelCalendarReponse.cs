using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HRTravelCalendarReponse : NumberRecord
    {
        [JsonPropertyName("employeeNo")]
        public string employeeNo { get; set; }

        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }
        [JsonPropertyName("id")]
        public int HRTravelCalendarID { get; set; }
        [JsonPropertyName("no")]
        public string HRTravelCalendarNo { get; set; }
        [JsonPropertyName("name")]
        public string HRTravelCalendarName { get; set; }
        [JsonPropertyName("calendarDate")]
        public Nullable<DateTime> HRTravelCalendarDate { get; set; }
        [JsonPropertyName("description")]
        public string HRTravelCalendarDesc { get; set; }
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> HRTravelCalendarFromDate { get; set; }
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> HRTravelCalendarToDate { get; set; }
        [JsonPropertyName("fk_travelType")]
        public int FK_HRTravelTypeID { get; set; }
        [JsonPropertyName("procStepId")]
        public int FK_ADApprovalProcStepID { get; set; } = 0;
        [JsonPropertyName("inboxId")]
        public int ADInboxItemID { get; set; } = 0;
        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; }
        [JsonPropertyName("realDay")]
        public int HRTravelCalendarRealDay { get; set; }
        [JsonPropertyName("fk_province_travelType")]
        public int FK_HRProvinceID { get; set; }
        [JsonPropertyName("sunDay")]
        public bool HRTravelCalendarSundayCheck { get; set; }
        [JsonPropertyName("feeCaculator")]
        public bool HRTravelCalendarNoCalCheck { get; set; }
    }
}
