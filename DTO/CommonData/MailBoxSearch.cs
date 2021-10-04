using gmc_api.Base.dto;
using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class MailBoxSearch : Paging
    {
        [JsonPropertyName("fromDate")]
        public Nullable<DateTime> fromDate { get; set; } = null;
        [JsonPropertyName("toDate")]
        public Nullable<DateTime> toDate { get; set; } = null;
        [JsonPropertyName("objectType")]
        public string objectType { get; set; } = "";
    }
}
