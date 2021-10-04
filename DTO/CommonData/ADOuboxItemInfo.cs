using System;

namespace gmc_api.DTO.CommonData
{
    public class ADOuboxItemInfo
    {
        public Nullable<DateTime> ADOutboxItemDate { get; set; } = DateTime.Now;
        public string AAStatus { get; set; } = "Alive";
        public int FK_ADApprovalProcID { get; set; }
        public int FK_ADApprovalProcStepID { get; set; }
        public string ADMailToUsers { get; set; }
        public string ADMailCCUsers { get; set; }
        public string type { get; set; }
        public string ADOutboxItemProtocol { get; set; }
        public string ADOutboxItemSubject { get; set; }
        public string ADOutboxItemMessage { get; set; }
        public int ADOutboxItemObjectID { get; set; }
        public string ADOutboxItemTableName { get; set; }
        public string ADOutboxItemDocApprovalStatusCombo { get; set; }
        public string ADOutboxItemAction { get; set; }
        public string ADOutboxItemTaskStatusCombo { get; set; }
        public string ADOutboxItemPriorityCombo { get; set; }
        public string ADOutboxItemDocType { get; set; }
        public string ADOutboxItemDocNo { get; set; }
        public int FK_ADFromUserID { get; set; }
        public int FK_HRFromEmployeeID { get; set; }


    }
}