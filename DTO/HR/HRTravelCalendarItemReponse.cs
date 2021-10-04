using System.Text.Json.Serialization;

namespace gmc_api.DTO.HR
{
    public class HRTravelCalendarItemReponse
    {
        [JsonPropertyName("id")]
        public int HRTravelCalendarItemID { get; set; }
        [JsonPropertyName("calendarId")]
        public int FK_HRTravelCalendarID { get; set; }
        [JsonPropertyName("employeeId")]
        public int FK_HREmployeeID { get; set; }
        [JsonPropertyName("note")]
        public string HRTravelCalendarNote { get; set; }
    }
}
