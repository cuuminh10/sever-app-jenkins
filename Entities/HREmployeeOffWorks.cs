using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{

    [Table("HREmployeeOffWorks")]
    public class HREmployeeOffWorks : FixFiveProps
    {
        [Key]
        public int HREmployeeOffWorkID { get; set; }
        public int FK_HREmployeeID { get; set; }
        public Nullable<DateTime> HREmployeeOffWorkFromDate { get; set; }
        public Nullable<DateTime> HREmployeeOffWorkToDate { get; set; }
        public string HREmployeeOffWorkTypeCombo { get; set; }
        public string HREmployeeOffWorkApproveUser { get; set; }
        public string HREmployeeOffWorkReasonDetail { get; set; }
        public Boolean HREmployeeOffWorkIsApprove { get; set; } = true;
        public decimal HREmployeeOffWorkDay { get; set; }
        public decimal HREmployeeActualOffWorkDay { get; set; }
        public int FK_HREmployeeLeaveTypeID { get; set; }
        public Nullable<DateTime> HREmployeeOffWorkCreateDate { get; set; } = DateTime.Now;
        public string HREmployeeOffWorkApprovedPerson { get; set; }
        public int HREmployeeOffWorkPeriod { get; set; }
        public int HREmployeeOffWorkFiscalYear { get; set; }
        public decimal HREmployeeOffWorkRegDays { get; set; }
        public Boolean AASelected { get; set; }
        public int FK_HRPositionID { get; set; }
        public int FK_HRSectionID { get; set; }
        public string HREmployeeOffWorkNote { get; set; }
        public string HREmployeeOffWorkStatus { get; set; }
        public string HREmployeeOffWorkUnApproveReason { get; set; }
        public string ApprovalStatusCombo { get; set; }
        public string HREmployeeOffWorkCancelReason { get; set; }
        public Nullable<DateTime> HREmployeeOffWorkCancelDate { get; set; }
        public string HREmployeeOffWorkCancelUser { get; set; }
        public string HREmployeeOffWorkReadUsers { get; set; }
        public int FK_ADApprovalProcID { get; set; }
        public string HREmployeeOffWorkNo { get; set; }
    }
}
