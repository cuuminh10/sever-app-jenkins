
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{
    public class LoginResponse
    {

        [JsonPropertyName("id")]
        public int ADUserID { get; set; }
        [JsonPropertyName("groupId")]
        public int ADUserGroupID { get; set; }
        [JsonPropertyName("contactId")]
        public int ADContactID { get; set; }
        [JsonPropertyName("username")]
        public string ADUserName { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("modules")]
        public List<RoleOfUser> moduleList { get; set; }
    }
}
