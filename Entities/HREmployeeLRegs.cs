using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("HREmployeeLRegs")]
    public class HREmployeeLRegs : FixFiveProps
    {
        [Key]
        public int HREmployeeLRegID { get; set; }
        public bool AASelected { get; set; }
        public int FK_HREmployeeID { get; set; }
        public decimal HREmployeeLRegDftDays { get; set; }
        public decimal HREmployeeLRegPreviousDays { get; set; }
        public decimal HREmployeeLRegExpDays { get; set; }
        public decimal HREmployeeLRegTotDays { get; set; }
        public decimal HREmployeeLRegLeaveDays { get; set; }
        public decimal HREmployeeLRegRemainDays { get; set; }
        public string HREmployeeLRegDesc { get; set; }
        public decimal HREmployeeLRegAnnualLeaveTot { get; set; }
        public decimal HREmployeeLRegNoxiouDays { get; set; }
        public decimal HREmployeeLRegAnnualLeaveOtherDays { get; set; }
        public int HREmployeeLRegYear { get; set; }
        public Nullable<DateTime> HREmployeeLRegDateCal { get; set; }
        public decimal HREmployeeLRegPreviousDayUsed { get; set; }
    }
}
