using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{

    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string ADUserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string ADPassword { get; set; }
    }
}
