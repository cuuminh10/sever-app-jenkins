using gmc_api.Base.dto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{
    public class UserCreateRequest : DtoGMCBase
    {
        [Required]
        [JsonPropertyName("groupId")]
        public int? ADUserGroupID { get; set; }
        [Required]
        [JsonPropertyName("contactId")]
        public int? ADContactID { get; set; }
        [Required]
        [JsonPropertyName("username")]
        public string ADUserName { get; set; }
        [Required]
        [JsonPropertyName("password")]
        public string ADPassword { get; set; }
    }
}
