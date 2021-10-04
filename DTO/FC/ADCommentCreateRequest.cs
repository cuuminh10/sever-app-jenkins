using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.FC
{
    public class ADCommentCreateRequest
    {
        [Required]
        [JsonPropertyName("content")]
        public string ADCommentName { get; set; }
        [JsonIgnore]
        public string ADCommentTable { get; set; }
        [JsonIgnore]
        public string ADCommentRefID { get; set; }
    }
}
