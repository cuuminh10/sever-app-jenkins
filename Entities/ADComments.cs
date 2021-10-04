using gmc_api.Base.dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ADComments")]
    public class ADComments : FixFiveProps
    {
        [Key]
        public int ADCommentID { get; set; }
        public string ADCommentName { get; set; }
        public string ADCommentTable { get; set; }
        public string ADCommentRefID { get; set; }
    }
}
