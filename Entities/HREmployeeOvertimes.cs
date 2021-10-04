using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("HREmployeeOvertimes")]
    public class HREmployeeOvertimes : FixFiveProps
    {
        [Key]
        public int HREmployeeOvertimeID { get; set; }
        public int FK_HREmployeeID { get; set; }
        public Nullable<DateTime> HREmployeeOvertimeFromDate { get; set; }
        public Nullable<DateTime> HREmployeeOvertimeToDate { get; set; }
        public string HREmployeeOvertimeApproveUser { get; set; } = "";
        public Boolean HREmployeeOvertimeIsApprove { get; set; } = true;
        public int FK_HROvertimeRateID { get; set; }
        public int FK_HRShiftID { get; set; }
        public string HREmployeeOvertimeTypeCombo { get; set; } = "";
        public string HREmployeeOvertimeApprovedPerson { get; set; } = "";
        public string HREmployeeOvertimeReasonDetail { get; set; }
        public int HREmployeeOvertimePeriod { get; set; }
        public int HREmployeeOvertimeFiscalYear { get; set; }
        public Boolean AASelected { get; set; }
        public Boolean HREmployeeOvertimeOverDayCheck { get; set; }
        public string ApprovalStatusCombo { get; set; }
        public int FK_ADApprovalProcID { get; set; }
        public string HREmployeeOvertimeNo { get; set; }
        public Boolean HREmployeeOvertimeCTCheck { get; set; }
        public decimal HREmployeeOvertimeBreakHour { get; set; }
        public decimal HREmployeeOvertimeHour { get; set; }
    }
}
