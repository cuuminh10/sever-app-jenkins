using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("HRPeriods")]
    public class HRPeriods : FixFiveProps
    {
        [Key]
        public int HRPeriodID { get; set; }
        public string HRPeriodNo { get; set; }
        public string HRPeriodName { get; set; }
        public Nullable<DateTime> HRPeriodDate { get; set; }
        public string HRPeriodDesc { get; set; }
        public bool AASelected { get; set; }
        public string HRPeriodYear { get; set; }
        public Nullable<DateTime> HRPeriodFromDate { get; set; }
        public Nullable<DateTime> HRPeriodToDate { get; set; }
        public bool HRPeriodCloseCheck { get; set; }
        public string HRPeriodNumber { get; set; }
        public float HRPeriodStdDay { get; set; }
    }
}
