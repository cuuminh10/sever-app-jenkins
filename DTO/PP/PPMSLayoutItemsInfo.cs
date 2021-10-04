using System;

namespace gmc_api.DTO.PP
{
    public class PPMSLayoutItemsInfo
    {
        public int PPMSLayoutItemID { get; set; }
        public string AAStatus { get; set; }
        public string AACreatedUser { get; set; }
        public string AAUpdatedUser { get; set; }
        public Nullable<DateTime> AACreatedDate { get; set; }
        public Nullable<DateTime> AAUpdatedDate { get; set; }
        public bool AASelected { get; set; }
        public string PPMSLayoutItemDesc { get; set; }
        public int FK_ICProductID { get; set; }
        public int FK_ICUOMID { get; set; }
        public int FK_ICStkUOMID { get; set; }
        public int FK_PPMSID { get; set; }
        public int FK_PPMSLayoutID { get; set; }
        public decimal PPMSLayoutItemQty { get; set; }
        public decimal PPMSLayoutItemStkQty { get; set; }
        public decimal PPMSLayoutItemExcQty { get; set; }
        public decimal PPMSLayoutItemFact { get; set; }
        public decimal PPMSLayoutItemRatioQty { get; set; }
    }
}
