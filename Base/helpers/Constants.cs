using System;
using System.Collections.Generic;

namespace gmc_api.Base.Helpers
{
    public class Constants
    {
        public const int DEFAULT_VALUE_INT = -12734658;
        public const string DEFAULT_VALUE_STRING = "UNDEFILED_VALUE";
        public const double DEFAULT_VALUE_DOUBLE = 0.123495687;
        public const decimal DEFAULT_VALUE_DECIMAL = -12734658;
        public static DateTime DEFAULT_VALUE_DATETIME = DateTime.MinValue;
        public static bool? DEFAULT_VALUE_BOOL = null;

        public static class ColumnSuffix
        {
            #region Item Suffix Column
            public const string cstItemDescColumnSuffix = "Desc";
            public const string cstItemTypeColumnSuffix = "Type";
            public const string cstItemUnitPriceColumnSuffix = "UnitPrice";
            public const string cstItemPriceColumnSuffix = "Price";
            public const string cstItemFUnitPriceColumnSuffix = "FUnitPrice";
            public const string cstItemFPriceColumnSuffix = "FPrice";
            public const string cstItemUnitCostColumnSuffix = "UnitCost";
            public const string cstItemIsStkItemColumnSuffix = "IsStkItm";
            public const string cstItemUOMColumnSuffix = "UOM";
            public const string cstItemStkUOMColumnSuffix = "StkUOM";
            public const string cstItemSerialNoColumnSuffix = "SerialNo";
            public const string cstItemSalePriceColumnSuffix = "SalePriceCombo";
            public const string cstItemFactColumnSuffix = "Fact";
            public const string cstItemOrgUnitPriceColumnSuffix = "OrgUnitPrice";
            public const string cstItemOrgFUnitPriceColumnSuffix = "OrgFUnitPrice";

            public const string cstItemQtyColumnSuffix = "Qty";
            public const string cstItemRemainQtyColumnSuffix = "RQty";
            public const string cstItemStkQtyColumnSuffix = "StkQty";

            public const string cstItemAdjFUnitPriceColumn = "AdjFUnitPrice";
            public const string cstItemAdjUnitPriceColumn = "AdjUnitPrice";

            public const string cstItemTaxFUnitPriceColumn = "TaxFUnitPrice";
            public const string cstItemTaxUnitPriceColumn = "TaxUnitPrice";

            public const string cstItemStkAdjQtyColumnSuffix = "AdjStkQty";
            public const string cstItemAdjUnitPricePctColumn = "AdjUnitPricePct";
            public const string cstItemAdjDiscPctColumn = "AdjDiscPct";
            public const string cstItemAdjDiscFAmtColumn = "AdjDiscFAmt";
            public const string cstItemAdjDiscAmtColumn = "AdjDiscAmt";
            public const string cstItemFDiscAmtTotColumn = "FDiscAmtTot";
            public const string cstItemDiscAmtTotColumn = "DiscAmtTot";

            public const string cstItemUnitWeightColumnSuffix = "UnitWeight";
            public const string cstItemUnitVolumnColumnSuffix = "UnitVolumn";

            public const string cstItemWeightColumnSuffix = "WTot";
            public const string cstItemVolumnColumnSuffix = "VTot";

            public const string cstItemGrossWeightColumnSuffix = "GrossWTot";
            public const string cstItemNetWeightColumnSuffix = "NetWTot";

            public const string cstItemTaxPercentColumnSuffix = "TxPct";
            public const string cstItemDiscountPercentColumnSuffix = "DiscPct";

            public const string cstItemFTaxAmountColumnSuffix = "FTxAmt";
            public const string cstItemTaxAmountColumnSuffix = "TxAmt";
            public const string cstItemFImpTaxAmountColumnSuffix = "FImpTxAmt";
            public const string cstItemShpAmtColumnSuffix = "ShpAmt";
            public const string cstItemMiscChargeAmtColumnSuffix = "MiscChargeAmt";
            public const string cstItemImpTaxAmountColumnSuffix = "ImpTxAmt";
            public const string cstItemFNetAmountColumnSuffix = "FNetAmt";
            public const string cstItemNetAmountColumnSuffix = "NetAmt";
            public const string cstItemFDiscountAmountColumnSuffix = "FDiscAmt";
            public const string cstItemDiscountAmountColumnSuffix = "DiscAmt";
            public const string cstItemAllocateDiscountAmountColumnSuffix = "AllocateDiscAmt";
            public const string cstItemAllocateDiscountFAmountColumnSuffix = "AllocateDiscFAmt";
            public const string cstItemFTotalAmountColumnSuffix = "FAmtTot";
            public const string cstItemTotalAmountColumnSuffix = "AmtTot";
            public const string cstItemTotalCostAmountColumnSuffix = "CostTot";
            public const string cstExcRateColumnSuffix = "ExcRate";
            #endregion

            #region Header Suffix Column
            public const string cstDiscountAmountColumnSuffix = "DiscAmt";
            public const string cstFreightAmountColumnSuffix = "FreightAmt";
            public const string cstShipAmountColumnSuffix = "ShpAmt";
            public const string cstMiscChargeAmountColumnSuffix = "MiscChargeAmt";
            public const string cstTotalItemPriceColumnSuffix = "ItmPriceTot";
            public const string cstTotalItemFPriceColumnSuffix = "ItmFPriceTot";
            public const string cstTotalItemAmountColumnSuffix = "ItmAmtTot";
            public const string cstTotalItemFAmountColumnSuffix = "ItmFAmtTot";
            public const string cstTotalItemDiscountAmountColumnSuffix = "ItmDiscAmtTot";
            public const string cstTotalItemDiscountFAmountColumnSuffix = "ItmDiscFAmtTot";
            public const string cstTotalItemNetAmountColumnSuffix = "NetAmtTot";
            public const string cstTotalItemNetFAmountColumnSuffix = "NetFAmtTot";
            public const string cstTotalItemTaxAmountColumnSuffix = "TxAmtTot";
            public const string cstTotalItemTaxFAmountColumnSuffix = "TxFAmtTot";
            public const string cstTotalItemImpTaxAmountColumnSuffix = "ImpTxAmtTot";
            public const string cstTotalAmountColumnSuffix = "AmtTot";
            public const string cstTotalFAmountColumnSuffix = "FAmtTot";
            public const string cstTotalRemainAmountColumnSuffix = "RAmtTot";
            public const string cstTotalRemainFAmountColumnSuffix = "RFAmtTot";
            public const string cstTotalWeightColumnSuffix = "Weight";
            public const string cstTotalVolumnColumnSuffix = "Volumn";
            #endregion
        }
        public static class MailBoxType
        {
            public const string INBOX = "inbox";
            public const string OUTBOX = "outbox";
            public const string BOTH = "BOTH";
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(INBOX) || data.Equals(OUTBOX))
                    return true;
                return false;
            }
        }

        public static class MailBoxProtocolType
        {
            public const string APPROVE = "Approval";
            public const string Message = "Message";
        }

        public static class FileUploadType
        {
            public const string JOB_TIKCET = "jobticket";
            public const string P_RESULT = "producResult";
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(JOB_TIKCET) || data.Equals(P_RESULT))
                    return true;
                return false;
            }
            public static string messageValidate()
            {
                return "Please in put correct type : " + JOB_TIKCET + " or " + P_RESULT;
            }
            public static Dictionary<string, string> UploadTable = new()
            {
                { JOB_TIKCET, "PPProductionOrdrs" },
                { P_RESULT, "PPProductionOrdrs" }
            };
            public static Dictionary<string, string> folderUpload = new()
            {
                { JOB_TIKCET, "jobticket" },
                { P_RESULT, "producResult" },
            };
        }
        public static class TableModule
        {
            public const string TravelCalendar = "TravelCalendar";
            public const string ProducResult = "ProductionFG";
            public static Dictionary<string, string> get = new()
            {
                { TravelCalendar, "HRTravelCalendars" },
                { ProducResult, "PPProductionOrdrs" },
                { "jobticket", "PPProductionOrdrs" },
                { "producResult", "PPProductionOrdrs" }
            };
        }
        public static class TableCommon
        {
            public static Dictionary<string, string> commonTable = new()
            {
                { "transConfig", "GLTranCfgs_Name" },
                { "phaseConfig", "PPPhaseCfgs_Name" },
                { "workOrder", "PPWOs" },
                { "moduleList", "STModules_Name" },
                { "jobticket", "PPProductionOrdrs" },
                { "producResult", "PPProductionOrdrs" },
                { "employeeLeaveTypes", "HREmployeeLeaveTypes_Desc" },
                { "overtimeRate", "HROvertimeRates_Desc" },
                { "shifts", "HRShifts_Name" },
                { "travelType", "HRTravelTypes_Name" },
                { "province", "HRProvinces_Name_HRTravelTypeID" },
            };
        }
        public static class ProductOrderStatus
        {
            public const string NEW = "New";
            public const string TRANSFERD = "Transfered";
            public const string TRANSFERING = "Transfering";
            public const string OVERDUE = "Overdue";
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(NEW) || data.Equals(TRANSFERD)
                    || data.Equals(TRANSFERING) || data.Equals(OVERDUE))
                    return true;
                return false;
            }
        }
        public static class ApproveStatus
        {
            public const string NEW = "New";
            public const string INPROCESS = "InProgress";
            public const string APROVED = "Approved";
            public const string APROVVING = "Approving";
            public const string REJECT = "Rejected";
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(NEW) || data.Equals(INPROCESS)
                    || data.Equals(APROVED) || data.Equals(REJECT) || data.Equals(APROVVING))
                    return true;
                return false;
            }
            public static string messageValidate()
            {
                return "Please in put correct type : " + APROVED + " or " + REJECT;
            }
            public static string messageValidateSearch()
            {
                return "Please in put correct type : " + APROVED + " or " + REJECT + " or " + INPROCESS;
            }
        }
        public static class ProductType
        {
            public const string JOB_TIKCET = "jobticket";
            public const string P_RESULT = "producResult";
            public static Dictionary<string, string> DBValue = new()
            {
                { JOB_TIKCET, "ProductionOrdr" },
                { P_RESULT, "ProductionFG" },
            };
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(JOB_TIKCET) || data.Equals(P_RESULT))
                    return true;
                return false;
            }

            public static string messageValidate()
            {
                return "Please in put correct type : " + JOB_TIKCET + " or " + P_RESULT;
            }
        }
        public static class GenerateNo
        {
            public const string TYPE_SYSTEMS = "System";
            public const string TYPE_COLUMNS = "Column";
            public const string TYPE_CONST = "Const";

            public const string SYS_DATE = "Date";
            public const string SYS_SEPER = "Seperator";
            public const string SYS_AUTO = "AutoNumber";
        }
        public static class ApproveType
        {
            public const string HR_OT = "EmployeeOvertime";
            public const string HR_TRAVEL = "TravelCalendar";
            public const string HR_OFF = "EmployeeOffWork";
            public const string PURCHASE_REQUEST = "PR";
            public const string PURCHASE_ORDER = "PO";
            public const string PAYMENT_REQUEST = "PmtReq";
            public const string SALE_ORDER = "SO";
            public static Dictionary<string, string> ApproveTitle = new()
            {
                { HR_OT, "Đăng ký tăng ca" },
                { HR_TRAVEL, "Lịch công tác" },
                { HR_OFF, "Đăng ký nghĩ phép" },
                { PURCHASE_REQUEST, "Phiếu đề nghị mua" },
                { PURCHASE_ORDER, "Đơn hàng bán" },
                { PAYMENT_REQUEST, "Phiếu đề nghị thanh toán" },
                { SALE_ORDER, "Đơn hàng bán" }
            };
            public static Dictionary<string, string> ApproveTable = new()
            {
                { HR_OT, "HREmployeeOvertimes" },
                { HR_TRAVEL, "HRTravelCalendars" },
                { HR_OFF, "HREmployeeOffWorks" },
                { PURCHASE_REQUEST, "APPRs" },
                { PURCHASE_ORDER, "APPOs" },
                { PAYMENT_REQUEST, "GLVouchers" },
                { SALE_ORDER, "ARSOs" }
            };
            public static bool isIn(string data)
            {
                if (string.IsNullOrEmpty(data))
                    return false;
                if (data.Equals(HR_OT) || data.Equals(HR_TRAVEL) || data.Equals(HR_OFF)
                    || data.Equals(PURCHASE_REQUEST) || data.Equals(PURCHASE_ORDER) || data.Equals(PAYMENT_REQUEST)
                    || data.Equals(SALE_ORDER))
                    return true;
                return false;
            }
            public static string messageValidate()
            {
                return "Please in put correct type : " + HR_OT + " or " + HR_TRAVEL + " or " + HR_OFF + " or " + PURCHASE_REQUEST + " or " + PURCHASE_ORDER
                     + " or " + PAYMENT_REQUEST + " or " + SALE_ORDER;
            }
        }

    }
}
