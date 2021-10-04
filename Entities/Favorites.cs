using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("ADUserShortCuts")]
    public class Favorites : FixFiveProps
    {
        [Key]
        public int ADUserShortCutID { get; set; }
        public int FK_ADUserID { get; set; }
        public String ADUserShortCutModule { get; set; }
    }
}
