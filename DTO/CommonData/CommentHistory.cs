using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gmc_api.DTO.CommonData
{
    public class CommentHistory
    {
        public string createUser { get; set; }
        public DateTime createDate { get; set; }
        public string content { get; set; }
    }
}
