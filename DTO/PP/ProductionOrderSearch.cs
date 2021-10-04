using gmc_api.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gmc_api.DTO.PP
{
    //Search jticket: Status, Work Center, WorkOrder, Phase
    //Group job: Status, Work Center, WorkOrder, Phase, Work Center, Date
    // Bỏ
    // Group KQSX: Status, Work Center, WorkOrder, Job Ticket, Phase, Date
    public class ProductionOrderSearch
    {
        public string status { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string workCenter { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string workOrder { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string jobTicket { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string phase { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string createDate { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string groupByColumn { get; set; } = Constants.DEFAULT_VALUE_STRING;
        public string groupByValue { get; set; } = Constants.DEFAULT_VALUE_STRING;
    }
}
