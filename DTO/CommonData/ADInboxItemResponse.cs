using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class ADInboxItemResponse : NumberRecord
    {
        public bool isEdit { get; set; } = false;
        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }

        [JsonPropertyName("id")]
        public int ADInboxItemID { get; set; }
        [JsonPropertyName("subject")]
        public string ADInboxItemSubject { get; set; }
        [JsonPropertyName("objectNo")]
        public string ADInboxItemDocNo { get; set; }
        [JsonPropertyName("reciveDate")]
        public Nullable<DateTime> ADInboxItemDate { get; set; }
        [JsonPropertyName("toUser")]
        public string ADMailToUsers { get; set; }
        [JsonPropertyName("ccUser")]
        public string ADMailCCUsers { get; set; }
        [JsonPropertyName("content")]
        public string ADInboxItemMessage { get; set; }
        [JsonIgnore]
        public int FK_ADFromUserID { get; set; }
        [JsonIgnore]
        public int FK_HRFromEmployeeID { get; set; }
        public string ADInboxItemDocApprovalStatusCombo { get; set; }
        public string ADMailReadUsers { get; set; }
        public string ADInboxItemTableName { get; set; }
        [JsonPropertyName("objectId")]
        public int ADInboxItemObjectID { get; set; }
        [JsonPropertyName("docType")]
        public string ADInboxItemDocType { get; set; }
        [JsonPropertyName("protocol")]
        public string ADInboxItemProtocol { get; set; }
        [JsonPropertyName("procID")]
        public int FK_ADApprovalProcID { get; set; }
        [JsonPropertyName("procStepID")]
        public int FK_ADApprovalProcStepID { get; set; }
        [JsonPropertyName("types")]
        public string ADApprovalTypeCombo { get; set; }
    }
}
