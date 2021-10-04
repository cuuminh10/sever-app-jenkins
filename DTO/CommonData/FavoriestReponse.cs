using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.CommonData
{
    public class FavoriestReponse
    {
        [JsonPropertyName("id")]
        public int ADUserShortCutID { get; set; }
        [JsonPropertyName("moduleName")]
        public String ADUserShortCutModule { get; set; }
    }
}
