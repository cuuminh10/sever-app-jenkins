using gmc_api.Base.dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ADAttachments")]
    public class ADAttachments : FixFiveProps
    {
        [Key]
        public int ADAttachmentID { get; set; }
        public string ADAttachmentName { get; set; }
        public string ADAttachmentPath { get; set; }
        public string ADAttachmentTable { get; set; }
        public string ADAttachmentRefID { get; set; }
    }
}
