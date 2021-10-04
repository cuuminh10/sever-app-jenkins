using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class ADOutboxItemResponse : NumberRecord
    {
        public bool isEdit { get; set; } = false;
        [JsonPropertyName("employeeFullName")]
        public string employeeFullName { get; set; }
        [JsonPropertyName("id")]
        public int ADOutboxItemID { get; set; }
        [JsonPropertyName("subject")]
        public string ADOutboxItemSubject { get; set; }
        [JsonPropertyName("objectNo")]
        public string ADOutboxItemDocNo { get; set; }
        [JsonPropertyName("reciveDate")]
        public Nullable<DateTime> ADOutboxItemDate { get; set; }
        [JsonPropertyName("toUser")]
        public string ADMailToUsers { get; set; }
        [JsonPropertyName("ccUser")]
        public string ADMailCCUsers { get; set; }
        [JsonPropertyName("content")]
        public string ADOutboxItemMessage { get; set; }
        [JsonIgnore]
        public int FK_ADFromUserID { get; set; }
        [JsonIgnore]
        public int FK_HRFromEmployeeID { get; set; }
        public string ADOutboxItemTaskStatusCombo { get; set; }
        [JsonPropertyName("readFlag")]
        public bool ADOutboxItemUnRead { get; set; }
        public string ADOutboxItemTableName { get; set; }
        [JsonPropertyName("objectId")]
        public int ADOutboxItemObjectID { get; set; }
        [JsonPropertyName("docType")]
        public string ADOutboxItemDocType { get; set; }
        [JsonPropertyName("protocol")]
        public string ADOutboxItemProtocol { get; set; }
        [JsonPropertyName("procID")]
        public int FK_ADApprovalProcID { get; set; }
        [JsonPropertyName("procStepID")]
        public int FK_ADApprovalProcStepID { get; set; }
        [JsonPropertyName("types")]
        public string ADApprovalTypeCombo { get; set; }
    }
}
