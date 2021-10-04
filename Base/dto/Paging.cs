using System;
using System.ComponentModel.DataAnnotations;

namespace gmc_api.Base.dto
{
    public class Paging
    {
        [Range(1, Int32.MaxValue)]
        public int pageNo { get; set; } = 1;
        [Range(1, Int32.MaxValue)]
        public int numberRows { get; set; } = 10;
    }
}
