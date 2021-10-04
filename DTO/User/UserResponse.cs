using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{
    public class UserResponse
    {
        [JsonPropertyName("id")]
        public int ADUserID { get; set; }
        [JsonPropertyName("groupId")]
        public int ADUserGroupID { get; set; }
        [JsonPropertyName("contactId")]
        public int ADContactID { get; set; }
        [JsonPropertyName("username")]
        public string ADUserName { get; set; }
    }
}
