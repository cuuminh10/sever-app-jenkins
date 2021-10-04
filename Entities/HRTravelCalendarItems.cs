using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("HRTravelCalendarItems")]
    public class HRTravelCalendarItems
    {
        [Key]
        public int HRTravelCalendarItemID { get; set; }
        public int FK_HRTravelCalendarID { get; set; }
        public int FK_HREmployeeID { get; set; }
        public string HRTravelCalendarNote { get; set; }
    }
}
