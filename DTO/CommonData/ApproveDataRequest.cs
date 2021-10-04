using System.ComponentModel.DataAnnotations;

namespace gmc_api.DTO.CommonData
{
    public class ApproveDataRequest
    {
        [Required]
        public string status { get; set; }
        [Required]
        public int objectId { get; set; }
        [Required]
        public int approveStepID { get; set; }
        public string reasion { get; set; } = "";
        [Required]
        public int inboxId { get; set; }
    }
}
