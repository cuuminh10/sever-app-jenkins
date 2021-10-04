using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("PPProductionOrdrEstRMs")]
    public class PPProductionOrdrEstRMs
    {
        [Key]
        public int PPProductionOrdrEstRMID { get; set; }
        public int FK_ICProductID { get; set; }
        public int FK_ICProductTypeID { get; set; }
        public string PPProductionOrdrEstRMSerialNo { get; set; }
        public bool PPProductionOrdrEstRMIsStkItm { get; set; }
        public string PPProductionOrdrEstRMType { get; set; }
        public string PPProductionOrdrEstRMDesc { get; set; }
        public decimal PPProductionOrdrEstRMFact { get; set; }
        public decimal PPProductionOrdrEstRMUnitPrice { get; set; }
        public decimal PPProductionOrdrEstRMUnitCost { get; set; }
        public decimal PPProductionOrdrEstRMQty { get; set; }
        public decimal PPProductionOrdrEstRMStkQty { get; set; }
        public decimal PPProductionOrdrEstRMRQty { get; set; }
        public decimal PPProductionOrdrEstRMPrice { get; set; }
        public decimal PPProductionOrdrEstRMAmtTot { get; set; }
        public decimal PPProductionOrdrEstRMCostTot { get; set; }
        public decimal PPProductionOrdrEstRMAmt { get; set; }
        public decimal PPProductionOrdrEstRMCAmt { get; set; }
        public int FK_ICFGProductID { get; set; }
        public int FK_PPPhaseCfgID { get; set; }
        public int FK_PPProductionOrdrID { get; set; }
        public int FK_PPWOID { get; set; }
        public string AAStatus { get; set; }
        public bool AASelected { get; set; }
        public int FK_ICUOMID { get; set; }
        public int FK_ICStkUOMID { get; set; }
        public decimal PPProductionOrdrEstRMRStkQty { get; set; }
        public int FK_ICWeightUOMID { get; set; }
        public int FK_ICVolumeUOMID { get; set; }
        public decimal PPProductionOrdrEstRMWTot { get; set; }
        public decimal PPProductionOrdrEstRMVTot { get; set; }
        public decimal PPProductionOrdrEstRMCQty { get; set; }
        public string PPProductionOrdrEstRMLotNo { get; set; }
        public string PPProductionOrdrEstRMFGLotNo { get; set; }
        public string PPProductionOrdrEstRMUOM { get; set; }
        public string PPProductionOrdrEstRMStkUOM { get; set; }
        public int FK_ARShpPlanItemID { get; set; }
        public int FK_ARSOID { get; set; }
        public int FK_ARSOItemID { get; set; }
        public int FK_PPRoutingID { get; set; }
        public int FK_PPWorkCenterID { get; set; }
        public int FK_PPWORID { get; set; }
        public decimal PPProductionOrdrEstRMROQty { get; set; }
        public int FK_ICProductParentID { get; set; }
        public decimal PPProductionOrdrEstRMBOMQty { get; set; }
        public decimal PPProductionOrdrEstRMParentQty { get; set; }
        public decimal PPProductionOrdrEstRMInvQty { get; set; }
        public decimal PPProductionOrdrEstRMPOQty { get; set; }
        public decimal PPProductionOrdrEstRMActAllocateQty { get; set; }
        public decimal PPProductionOrdrEstRMBalanceQty { get; set; }
        public decimal PPProductionOrdrEstRMAllocateQty { get; set; }
        public int FK_ICStockID { get; set; }
    }
}
