using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ADOutboxItems")]
    public class ADOutboxItems
    {
        [Key]
        public int ADOutboxItemID { get; set; }
        public string AAStatus { get; set; }
        public string ADOutboxItemSubject { get; set; }
        public string ADOutboxItemDocNo { get; set; }
        public string ADOutboxItemDocType { get; set; }
        public Nullable<DateTime> ADOutboxItemDate { get; set; }
        public string ADOutboxItemMessage { get; set; }
        public string ADOutboxItemProtocol { get; set; }
        public string ADOutboxItemPriorityCombo { get; set; }
        public int FK_ADFromUserID { get; set; }
        public int FK_HRFromEmployeeID { get; set; }
        public string ADMailToUsers { get; set; }
        public string ADMailCCUsers { get; set; }
        public string ADOutboxItemTaskStatusCombo { get; set; }
        public bool ADOutboxItemUnRead { get; set; } = true;
        public string ADOutboxItemAction { get; set; }
        public int FK_ADApprovalProcID { get; set; }
        public string ADOutboxItemRemark { get; set; } = "";
        public int FK_ADApprovalProcStepID { get; set; }
        public int FK_ADToUserID { get; set; } = 0;
        public string ADOutboxItemDocApprovalStatusCombo { get; set; }
        public string ADOutboxItemTableName { get; set; }
        public int ADOutboxItemObjectID { get; set; }
        public string ADApprovalTypeCombo { get; set; } = "";
    }
}
