using gmc_api.DTO.FC;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace gmc_api.DTO.PP
{
    public class JobTicketDetail : JobTicketDetailBasic
    {
        [JsonPropertyName("detail")]
        public List<JobTicketItemsInfo> detail { get; set; }
        [JsonPropertyName("attach")]
        public IEnumerable<ADAttachmentReponse> attach { get; set; }
        [JsonPropertyName("comment")]
        public IEnumerable<ADCommentReponseCus> comment { get; set; }
        [JsonPropertyName("document")]
        public IEnumerable<ADDocumentReponse> document { get; set; }
    }
}
