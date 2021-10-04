using System;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.FC
{
    public class ADDocumentReponse
    {
        public int id { get; set; }
        public string realName { get; set; }
        public string saveName { get; set; }
        public string comment { get; set; }
        public string types { get; set; }
        public string createUser { get; set; }
        public DateTime createDate { get; set; }
    }
}
