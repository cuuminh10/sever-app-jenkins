using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class PPProductionOrdrResponse
    {
        [JsonPropertyName("id")]
        public int PPProductionOrdrID { get; set; }
        [JsonPropertyName("createUser")]
        public String AACreatedUser { get; set; }
        [JsonPropertyName("createDate")]
        public Nullable<DateTime> AACreatedDate { get; set; } = DateTime.Now;
        [JsonPropertyName("postStatus")]
        public String AAPostStatus { get; set; }
        [JsonPropertyName("postNo")]
        public String AALastPostNo { get; set; }
        [JsonPropertyName("lastPostDate")]
        public Nullable<DateTime> AALastPostDate { get; set; }
        [JsonPropertyName("no")]
        public String PPProductionOrdrNo { get; set; }
        [JsonPropertyName("name")]
        public String PPProductionOrdrName { get; set; }
        [JsonPropertyName("description")]
        public String PPProductionOrdrDesc { get; set; }
        [JsonPropertyName("ordDate")]
        public DateTime PPProductionOrdrDate { get; set; }
        [JsonPropertyName("ordStatus")]
        public String PPProductionOrdrStatus { get; set; }
        [JsonPropertyName("employeeName")]
        public string employeeName { get; set; }
        [JsonPropertyName("woNo")]
        public string woNo { get; set; }

    }
}
