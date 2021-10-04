using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gmc_api.DTO.CommonData
{
    public class AproveChart
    {
        public int Opens { get; set; }
        public int Inprocess { get; set; }
        public int Reject { get; set; }
        public int Approved { get; set; }

    }
}
