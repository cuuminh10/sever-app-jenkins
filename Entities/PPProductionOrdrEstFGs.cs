﻿using gmc_api.Base.dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gmc_api.Entities
{
    [Table("PPProductionOrdrEstFGs")]
    public class PPProductionOrdrEstFGs : FixFiveProps
    {
        [Key]
        public int PPProductionOrdrEstFGID { get; set; }
        public int FK_ICProductID { get; set; }
        public int FK_ICStockID { get; set; }
        public int FK_PPProductionOrdrID { get; set; }
        public int FK_PPWOID { get; set; }
        public int FK_PPPhaseCfgID { get; set; }
        public decimal PPProductionOrdrEstFGQty { get; set; }
        public decimal PPProductionOrdrEstFGMFQty { get; set; }
        public decimal PPProductionOrdrEstFGMFRQty { get; set; }
        public decimal PPProductionOrdrEstFGQCQty { get; set; }
        public decimal PPProductionOrdrEstFGRQty { get; set; }
        public int FK_PPWorkCenterID { get; set; }
        public int FK_PPRoutingID { get; set; }
        public int FK_ICUOMID { get; set; }
        public int FK_ICStkUOMID { get; set; }
        public decimal PPProductionOrdrEstFGStkQty { get; set; }
        public decimal PPProductionOrdrEstFGRStkQty { get; set; }
        public int FK_ICWeightUOMID { get; set; }
        public int FK_ICVolumeUOMID { get; set; }
        public decimal PPProductionOrdrEstFGWTot { get; set; }
        public decimal PPProductionOrdrEstFGVTot { get; set; }
        public decimal PPProductionOrdrEstFGFact { get; set; }
        public decimal PPProductionOrdrEstFGExcQty { get; set; }
        public decimal PPProductionOrdrEstFGOrgQty { get; set; }
        public int FK_ICOrgUOMID { get; set; }
        public decimal PPProductionOrdrEstFGCQty { get; set; }
        public int FK_PPNormID { get; set; }
        public string PPProductionOrdrEstFGLotNo { get; set; }
        public int PPProductionOrdrEstFGWeek { get; set; }
        public int PPProductionOrdrEstFGPeriod { get; set; }
        public int PPProductionOrdrEstFGYear { get; set; }
        public int FK_ARShpPlanItemID { get; set; }
        public int FK_ARSOID { get; set; }
        public int FK_ARSOItemID { get; set; }
        public string PPProductionOrdrEstFGDesc { get; set; }
        public decimal PPProductionOrdrEstFGNCRQty { get; set; }
        public string PPProductionOrdrEstFGNote { get; set; }
        public decimal PPProductionOrdrEstFGSetupQty { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstEndDate { get; set; }
        public int FK_PPResourceID { get; set; }
        public int FK_PPResourceGroupID { get; set; }
        public decimal PPProductionOrdrEstFGRcpQty { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstFGFODate { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstFGPlanDate { get; set; }
        public int FK_PPWORID { get; set; }
        public decimal PPProductionOrdrEstFGRcpCQty { get; set; }
        public decimal PPProductionOrdrEstFGCAPQty { get; set; }
        public decimal PPProductionOrdrEstFGCloseQty { get; set; }
        public int PPProductionOrdrEstFGOrder { get; set; }
        public decimal PPProductionOrdrEstFGLayout { get; set; }
        public int FK_ICLayoutUOMID { get; set; }
        public decimal PPProductionOrdrEstFGLayoutQty { get; set; }
        public int PPProductionOrdrEstFGLine { get; set; }
        public int FK_PPCostCenterID { get; set; }
        public int FK_PPShiftID { get; set; }
        public int FK_PPProductionOrdrEstFGID { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstFGEndTime { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstFGStarTime { get; set; }
        public decimal PPProductionOrdrEstFGDownTimeQty { get; set; }
        public decimal PPProductionOrdrEstFGProductPrice { get; set; }
        public decimal PPProductionOrdrEstFGPeopleQty { get; set; }
        public int FK_ICFGProductID { get; set; }
        public decimal PPProductionOrdrEstFGProductFGQty { get; set; }
        public int FK_ICLayoutProductID { get; set; }
        public decimal PPProductionOrdrEstFGCAPSetupQty { get; set; }
        public string GLTOF01Combo { get; set; }
        public string GLTOF02Combo { get; set; }
        public string GLTOF03Combo { get; set; }
        public string GLTOF04Combo { get; set; }
        public string GLTOF05Combo { get; set; }
        public string GLTOF06Combo { get; set; }
        public string GLTOF07Combo { get; set; }
        public string GLTOF08Combo { get; set; }
        public string GLTOF09Combo { get; set; }
        public string GLTOF10Combo { get; set; }
        public string GLTOF11Combo { get; set; }
        public string GLTOF12Combo { get; set; }
        public string GLTOF13Combo { get; set; }
        public string GLTOF14Combo { get; set; }
        public string GLTOF15Combo { get; set; }
        public int FK_ICProductRootID { get; set; }
        public int FK_PPWOEstFGItemID { get; set; }
        public decimal PPProductionOrdrEstFGLaborQty { get; set; }
        public decimal PPProductionOrdrEstFGLaborHrs { get; set; }
        public decimal PPProductionOrdrEstFGMachineHrs { get; set; }
        public string PPProductionOrdrEstFGQCStatusCombo { get; set; }
        public string PPProductionOrdrEstFGQCDesc { get; set; }
        public decimal PPProductionOrdrEstFGQCValidQty { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstQCGoodsDate { get; set; }
    }
}
