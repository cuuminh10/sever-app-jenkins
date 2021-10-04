using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class FavoriestCreateRequest
    {
        [Required]
        [JsonPropertyName("moduleName")]
        public string ADUserShortCutModule { get; set; }
        [JsonIgnore]
        public int FK_ADUserID { get; set; }
    }
}
