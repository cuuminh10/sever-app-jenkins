using System;

namespace gmc_api.DTO.CommonData
{
    public class ADInboxItemInfo
    {
        public Nullable<DateTime> ADInboxItemDate { get; set; } = DateTime.Now;
        public string AAStatus { get; set; } = "Alive";
        public int FK_ADApprovalProcID { get; set; }
        public int FK_ADApprovalProcStepID { get; set; }
        public string ADMailToUsers { get; set; }
        public string ADMailCCUsers { get; set; }
        public string type { get; set; }
        public string ADInboxItemProtocol { get; set; }
        public string ADInboxItemSubject { get; set; }
        public string ADInboxItemMessage { get; set; }
        public int ADInboxItemObjectID { get; set; }
        public string ADInboxItemTableName { get; set; }
        public string ADInboxItemDocApprovalStatusCombo { get; set; }
        public string ADInboxItemAction { get; set; }
        public string ADInboxItemTaskStatusCombo { get; set; }
        public string ADInboxItemPriorityCombo { get; set; }
        public string ADInboxItemDocType { get; set; }
        public string ADInboxItemDocNo { get; set; }
        public int FK_ADFromUserID { get; set; }
        public int FK_HRFromEmployeeID { get; set; }


    }
}