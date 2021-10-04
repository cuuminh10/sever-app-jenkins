using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.Payment
{
    public class GLVoucherPaymentInfo : NumberRecord
    {
        [JsonPropertyName("employeeNo")]
        public string employeeNo { get; set; }

        [JsonPropertyName("employeeName")]
        public string employeeFullName { get; set; }
        [JsonPropertyName("nccName")]
        public string nccName { get; set; }

        [JsonPropertyName("id")]
        public int GLVoucherID { get; set; }
        [JsonPropertyName("no")]
        public string GLVoucherNo { get; set; }
        [JsonPropertyName("sendDate")]
        public Nullable<DateTime> GLVoucherDate { get; set; }
        [JsonPropertyName("description")]
        public string GLVoucherDesc { get; set; }
        [JsonPropertyName("summary")]
        public decimal GLVoucherAmtTot { get; set; }
        [JsonPropertyName("summaryF")]
        public decimal GLVoucherFAmtTot { get; set; }
        [JsonPropertyName("payEmployeeName")]
        public string GLOutPmtPayToName { get; set; }
        [JsonPropertyName("customerName")]
        public string ARCustomerName { get; set; }
        [JsonPropertyName("lc_status")]
        public string ApprovalStatusCombo { get; set; } = "New";
        [JsonPropertyName("procStepId")]
        public int FK_ADApprovalProcStepID { get; set; } = 0;
        [JsonPropertyName("inboxId")]
        public int ADInboxItemID { get; set; } = 0;

        public int displayReject { get; set; } = 0;
    }
}
