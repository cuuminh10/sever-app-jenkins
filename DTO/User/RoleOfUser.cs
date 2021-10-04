using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{
    public class RoleOfUser
    {
        [JsonPropertyName("moduleName")]
        public string moduleName { get; set; }
        [JsonPropertyName("moduleId")]
        public int moduleId { get; set; }
    }
}
