using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.DTO.PP
{
    [Table("ADUsers")]
    public class User : DtoGMCBase
    {
        [Key]
        public int ADUserID { get; set; }
        public string AAStatus { get; set; }
        public int ADUserGroupID { get; set; }
        public int ADContactID { get; set; }
        public string ADUserName { get; set; }
        public string ADPassword { get; set; }
        public string ADProfileDirectory { get; set; }
        public string ADUserStyle { get; set; }
        public string ADUserStyleSkin { get; set; }
        public string ADUserCurLang { get; set; }
        public Boolean ADUserIsLockedCheck { get; set; }
        public string ADUserLockedInfo { get; set; }
    }
}
