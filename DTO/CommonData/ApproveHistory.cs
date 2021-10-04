using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gmc_api.DTO.CommonData
{
    public class ApproveHistory
    {
        public string approveNo { get; set; }
        public string approveStatus { get; set; }
        public string approveUser { get; set; }
        public string approvePos { get; set; }
        public string remark { get; set; }
        public Nullable<DateTime> approveDate { get; set; }
    }
}
