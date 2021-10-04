using System.Text.Json.Serialization;

namespace gmc_api.DTO.FC
{
    public class ADCommentReponseCus : ADCommentReponse
    {
        [JsonPropertyName("avatarUrl")]
        public string avatarUrl { get; set; }
    }
}
