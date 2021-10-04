using System;

namespace gmc_api.DTO.PP
{
    public class PPProductionOrdrCreate
    {
        public String AAPostStatus { get; set; }
        public String AALastPostNo { get; set; }
        public Nullable<DateTime> AALastPostDate { get; set; }
        public String PPProductionOrdrNo { get; set; }
        public String PPProductionOrdrName { get; set; }
        public String PPProductionOrdrDesc { get; set; }
        public Nullable<DateTime> PPProductionOrdrDate { get; set; }
        public String PPProductionOrdrStatus { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstDate { get; set; }
        public Nullable<DateTime> PPProductionOrdrActDate { get; set; }
        public int FK_HREmployeeID { get; set; }
        public int FK_PPWOID { get; set; }
        public int FK_PPPhaseCfgID { get; set; }
        public int FK_ICFromStockID { get; set; }
        public int FK_ICToStockID;
        public Nullable<DateTime> PPProductionOrdrEstStartDate { get; set; }
        public Nullable<DateTime> PPProductionOrdrEstEndDate { get; set; }
        public Nullable<DateTime> PPProductionOrdrActStartDate { get; set; }
        public Nullable<DateTime> PPProductionOrdrActEndDate { get; set; }
        public String PPProductionOrdrTypeCombo { get; set; }
        public String PPProductionOrdrLotNo { get; set; }
        public bool AASelected { get; set; } = true;
        public String AACompanyTypeCombo { get; set; }
        public String AAOrgDocNo { get; set; }
        public int FK_PPProductionOrdrParentID { get; set; }
        public int FK_PPWorkCenterID { get; set; }
        public int FK_PPResourceID { get; set; }
        public Nullable<DateTime> PPProductionOrdrJrnDate { get; set; }
        public int FK_BRBranchID { get; set; }
        public String PPProductionOrdrConvertTypeCombo { get; set; }
        public int FK_PPPrdQCID { get; set; }
        public String PPProductionOrdrJrnNo { get; set; }
        public int FK_GLTranCfgID { get; set; }
        public int FK_ICConvertProductID { get; set; }
        public String PPProductionOrdrGroupCombo { get; set; }
        public int FK_HRMachineID { get; set; }
        public int FK_ICProductRootID { get; set; }
    }
}
