using System;

namespace gmc_api.DTO.PP
{
    public class ProdRstDetailBasic
    {
        public int id { get; set; }
        public string no { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime ordDate { get; set; }
        public string jobTicketNo { get; set; }
        public string woNo { get; set; }
        public string workCenterName { get; set; }
        public string phaseNo { get; set; }

    }
}
