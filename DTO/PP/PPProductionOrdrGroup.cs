using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class PPProductionOrdrGroup
    {
        public string name { get; set; }
        public int counts { get; set; } = 0;
    }
}
