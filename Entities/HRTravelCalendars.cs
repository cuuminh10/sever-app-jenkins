using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("HRTravelCalendars")]
    public class HRTravelCalendars : FixFiveProps
    {
        [Key]
        public int HRTravelCalendarID { get; set; }
        public string HRTravelCalendarNo { get; set; }
        public string HRTravelCalendarName { get; set; }
        public Nullable<DateTime> HRTravelCalendarDate { get; set; }
        public string HRTravelCalendarDesc { get; set; }
        public Nullable<DateTime> HRTravelCalendarFromDate { get; set; }
        public Nullable<DateTime> HRTravelCalendarToDate { get; set; }
        public bool AASelected { get; set; } = true;
        public int FK_HRTravelTypeID { get; set; }
        public string HRTravelCalendarPlace { get; set; } = "";
        public int FK_ADApprovalProcID { get; set; }
        public string ApprovalStatusCombo { get; set; }
        public int HRTravelCalendarTempDay { get; set; }
        public int HRTravelCalendarRealDay { get; set; }
        public string HRTravelCalendarStatusCombo { get; set; } = "New";
        public int FK_HRProvinceID { get; set; }
        public int FK_PMProjectID { get; set; } = 0;
        public bool HRTravelCalendarSundayCheck { get; set; }
        public bool HRTravelCalendarNoCalCheck { get; set; }
        public int FK_HRDepartmentID { get; set; }
    }
}
