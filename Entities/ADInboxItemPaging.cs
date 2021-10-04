using System;
using System.ComponentModel.DataAnnotations;

namespace gmc_api.Entities
{
    public class ADInboxItemPaging
    {
        [Key]
        public int ADInboxItemID { get; set; }
        public string AAStatus { get; set; }
        public string ADInboxItemSubject { get; set; }
        public string ADInboxItemDocNo { get; set; }
        public string ADInboxItemDocType { get; set; }
        public Nullable<DateTime> ADInboxItemDate { get; set; }
        public string ADInboxItemMessage { get; set; }
        public string ADInboxItemProtocol { get; set; }
        public string ADInboxItemPriorityCombo { get; set; }
        public int FK_ADFromUserID { get; set; }
        public int FK_HRFromEmployeeID { get; set; }
        public string ADMailToUsers { get; set; }
        public string ADMailCCUsers { get; set; }
        public string ADInboxItemTaskStatusCombo { get; set; }
        public string ADInboxItemAction { get; set; }
        public int FK_ADApprovalProcID { get; set; }
        public string ADInboxItemRemark { get; set; }
        public int FK_ADApprovalProcStepID { get; set; }
        public string ADInboxItemDocApprovalStatusCombo { get; set; }
        public Nullable<DateTime> ADInboxItemDeadline { get; set; }
        public string ADMailReadUsers { get; set; }
        public string ADInboxItemTableName { get; set; }
        public int ADInboxItemObjectID { get; set; }
        public string ADApprovalTypeCombo { get; set; }
        public int totalRows { get; set; }
    }
}
