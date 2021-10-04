using gmc_api.Base.dto;
using gmc_api.Base.Helpers;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.User
{
    public class UserUpdateRequest : DtoGMCBase
    {
        [JsonIgnore]
        public int ADUserID { get; set; }

        [JsonPropertyName("groupId")]
        public int ADUserGroupID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("contactId")]
        public int ADContactID { get; set; } = Constants.DEFAULT_VALUE_INT;
        [JsonPropertyName("username")]
        public string ADUserName { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("password")]
        public string ADPassword { get; set; } = Constants.DEFAULT_VALUE_STRING;
        [JsonPropertyName("lockCheck")]
        public bool? ADUserIsLockedCheck { get; set; } = null;
    }
}
