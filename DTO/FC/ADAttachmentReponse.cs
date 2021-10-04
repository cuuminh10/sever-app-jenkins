using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.FC
{
    public class ADAttachmentReponse
    {
        [JsonPropertyName("id")]
        public int ADAttachmentID { get; set; }
        [JsonPropertyName("realName")]
        public string ADAttachmentName { get; set; }
        [JsonPropertyName("saveName")]
        public string ADAttachmentPath { get; set; }
        [JsonPropertyName("createUser")]
        public string AACreatedUser { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime AACreatedDate { get; set; }
    }
}
