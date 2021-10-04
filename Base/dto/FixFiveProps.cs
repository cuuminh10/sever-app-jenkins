using System;

namespace gmc_api.Base.dto
{
    public class FixFiveProps
    {
        public String AAStatus { get; set; } = "Alive";
        public String AACreatedUser { get; set; }
        public String AAUpdatedUser { get; set; }
        public Nullable<DateTime> AACreatedDate { get; set; } = DateTime.Now;
        public Nullable<DateTime> AAUpdatedDate { get; set; } = DateTime.Now;
    }
}
