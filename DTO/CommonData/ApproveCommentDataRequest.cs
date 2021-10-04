using System.ComponentModel.DataAnnotations;

namespace gmc_api.DTO.CommonData
{
    public class ApproveCommentRequest
    {
        [Required]
        public int inboxId { get; set; } = 0;
        [Required]
        public string comment { get; set; } = "";
        public int objectId { get; set; } = 0;
    }
}
