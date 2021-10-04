using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.FC
{
    public class ADCommentReponse
    {
        [JsonPropertyName("id")]
        public int ADCommentID { get; set; }
        [JsonPropertyName("comment")]
        public string ADCommentName { get; set; }
        [JsonPropertyName("createUser")]
        public string AACreatedUser { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime AACreatedDate { get; set; }
    }
}
