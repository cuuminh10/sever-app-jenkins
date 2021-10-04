namespace gmc_api.DTO.CommonData
{
    public class ApproveDataInfo
    {
        public int approveId { get; set; }
        public int approveStepId { get; set; }
        public string approveUser { get; set; }
        public string ccUser { get; set; }
        public long rowNumber { get; set; }
        public int nextApproveStepId { get; set; }
        public int currentLevel { get; set; }
        public int preLevel { get; set; }
        public int nextLevel { get; set; }
    }
}