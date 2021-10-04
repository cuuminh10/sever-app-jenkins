using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gmc_api.DTO.CommonData
{
    public class ADDocHistoryInfo
    {
        public int ADDocHistoryID { get; set; }
        public string AAStatus { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public Nullable<DateTime> Date { get; set; } = DateTime.Now;
        public string UserName { get; set; }
        public string Action { get; set; }
        public int FK_ADApprovalProcID { get; set; }
        public int FK_ADApprovalProdStepID { get; set; }
        public string Remark { get; set; }
        public string TableName { get; set; }
        public int ObjectID { get; set; }
        public string ADApprovalTypeCombo { get; set; }
        public string PositionNo { get; set; }
    }
}
