using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.dto;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using gmc_api.DTO.AR;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.Payment;
using gmc_api.DTO.PO;
using gmc_api.DTO.PR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Repositories
{
    public interface ISimpleReponsitory : IRepositoriesBase<DtoGMCBase, DtoGMCBase>
    {
        public List<APPRInfo> getAPPRMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public List<APPRInfo> getAPPRApproveData(string userName, ApproveSearchBase consdition);
        public List<APPRItemInfo> getDetailAPPRItems(int parrentId);

        public List<APPOInfo> getAPPOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public List<APPOInfo> getAPPOApproveData(string userName, ApproveSearchBase consdition);
        public List<APPOItemInfo> getDetailAPPOItems(int parrentId);

        public List<ARSOItemInfo> getDetailSOItems(int parrentId);
        public List<ARSOInfo> getSOApproveData(string userName, ApproveSearchBase consdition);
        public List<ARSOInfo> getSOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);

        public List<GLVoucherPaymentItemInfo> getDetailVoucherPaymentItems(int parrentId);
        public List<GLVoucherPaymentInfo> getVoucherPaymentApproveData(string userName, ApproveSearchBase consdition);
        public List<GLVoucherPaymentInfo> getVoucherPaymentMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public AproveChart collectionData(string userName, int employeeId, int type, string objType);
        public OnePropIntReturn checkNextLevelApprove(int aPPOID, int aDInboxItemID, string tableName);
        public TwoPropReturn getLastAproveLevel(int proId);
        List<OnePropStringReturn> getUserByPosition(List<string> tmpCCUser);
    }
    public class SimpleReponsitory : RepositoriesBaseImpl<DtoGMCBase, DtoGMCBase>, ISimpleReponsitory
    {
        private readonly GMCContext _context;
        private readonly IMapper _mapper;
        public SimpleReponsitory(GMCContext context, IMapper mapper) : base(context, "Empty", "EmptyID")
        {
            _context = context;
            _mapper = mapper;
        }

        public List<OnePropStringReturn> getUserByPosition(List<string> tmpCCUser)
        {
            var sqlGetUserByPostion = string.Format(@"select distinct u.ADUserName dataReturn from HRPositions hrp
inner join HREmployees hre on hrp.HRPositionID = hre.FK_HRPositionID AND hre.AAStatus = 'Alive'
inner join ADUsers u on u.ADUserID = hre.FK_ADUserID AND u.AAStatus = 'Alive'
WHERE hrp.HRPositionNo in ({0}) and hrp.AAStatus = 'Alive'", string.Join(", ", tmpCCUser.Select(s => string.Format("'{0}'", s))));
            var rsGetUserByPostion = _context.OnePropStringReturn.FromSqlRaw(sqlGetUserByPostion).ToList<OnePropStringReturn>();
            return rsGetUserByPostion;
        }

        #region PR
        public List<APPRInfo> getAPPRMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("details.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
details.APPRID,
details.APPRNo,
details.APPRDate,
details.APPRDesc,
isnull(aps.APSupplierName,'') nccName,
details.ApprovalStatusCombo,
0 FK_ADApprovalProcStepID,
0 displayReject,
0 ADInboxItemID from APPRs details  
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
left join APSuppliers aps on aps.APSupplierID = details.FK_APSupplierID and aps.AAStatus = 'Alive'
WHERE  details.AAStatus = 'Alive'  {0} AND {1} 
ORDER BY details.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" details.APPRID = {0} ", consdition.id) : string.Format(@" (details.AACreatedUser = N'{0}' OR details.FK_HREmployeeID = {1}) ", userName, employeeId), offset, consdition.numberRows);

            return _context.APPRInfo.FromSqlRaw(sqlBuilding).ToList<APPRInfo>();
        }

        public List<APPRInfo> getAPPRApproveData(string userName, ApproveSearchBase consdition)
        {
            string statusApprove = "";
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("ib.ADInboxItemDate", consdition.fromDate, consdition.toDate);
            statusApprove = Utils.SelectStatusApprove(consdition.ApprovalStatusCombo, statusApprove);
            var detailStatus = "";
            if(consdition.ApprovalStatusCombo == ApproveStatus.INPROCESS)
            {
                detailStatus = " ('InProgress', 'Approving')";
            }
            var sqlBuilding = String.Format(@"SELECT totalRows = COUNT(*) OVER(), 
tmp.employeeNo,
employeeFullName,APPRID,APPRNo,APPRDate,APPRDesc,nccName,displayReject,
ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID
FROM (
SELECT  ib.ADInboxItemDate,
                            ISNULL(hre.HREmployeeNo, '') employeeNo,
                            ISNULL(hre.HREmployeeName, '') employeeFullName,
                            details.APPRID,
                            details.APPRNo,
                            details.APPRDate,
                            details.APPRDesc,
                            isnull(aps.APSupplierName, '') nccName,
                            details.ApprovalStatusCombo,
                            ib.FK_ADApprovalProcStepID,
                            ib.ADInboxItemID,
                            0 displayReject,
							ROW_NUMBER() OVER(PARTITION BY details.APPRID
                            ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM APPRs details
INNER JOIN ADInboxiTEMS ib ON details.APPRID = ib.ADInboxItemObjectID
LEFT JOIN APSuppliers aps ON aps.APSupplierID = details.FK_APSupplierID
AND aps.AAStatus = 'Alive'
LEFT JOIN HREmployees hre ON hre.HREmployeeID = details.FK_HREmployeeID AND hre.AAStatus = 'Alive'
where ib.ADInboxItemTableName = 'APPRs'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2} ) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus!=""?string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus):"");
            return _context.APPRInfo.FromSqlRaw(sqlBuilding).ToList();
        }

        public List<APPRItemInfo> getDetailAPPRItems(int parrentId)
        {
            var sqlBuilding = String.Format(@"select icp.ICProductNo, icp.ICProductName, apri.APPRItemQty, icu.ICUOMName,apri.APPRItemStkQty,
icu2.ICUOMName ICUOMNameStk, apri.APPRItemArrivalDate from APPRItems apri
inner join APPRs apr on apri.FK_APPRID = apr.APPRID AND apri.AAStatus = 'Alive'
inner join ICProducts icp on icp.ICProductID = apri.FK_ICProductID AND icp.AAStatus = 'Alive'
inner join ICUOMs icu on icu.ICUOMID = apri.FK_ICUOMID AND icu.AAStatus = 'Alive'
inner join ICUOMs icu2 on icu2.ICUOMID = apri.FK_ICStkUOMID and icu2.AAStatus = 'Alive'
where apr.APPRID = {0} and apr.AAStatus = 'Alive' order by APPRItemID", parrentId);
            return _context.APPRItemInfo.FromSqlRaw(sqlBuilding).ToList<APPRItemInfo>();
        }

        #endregion

        #region PO
        public List<APPOInfo> getAPPOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("details.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
details.APPOID,
details.APPONo,
details.APPODate,
details.APPODesc,
isnull(aps.APSupplierName,'') nccName,
details.APPOAmtTot,
details.ApprovalStatusCombo,
0 FK_ADApprovalProcStepID,
0 displayReject,
0 ADInboxItemID from APPOs details 
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
left join APSuppliers aps on aps.APSupplierID = details.FK_APSupplierID and aps.AAStatus = 'Alive'
WHERE  details.AAStatus = 'Alive'  {0} AND {1}
ORDER BY details.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" details.APPOID = {0} ", consdition.id) : string.Format(@" (details.AACreatedUser = N'{0}' OR details.FK_HREmployeeID = {1})  ", userName, employeeId), offset, consdition.numberRows);


            return _context.APPOInfo.FromSqlRaw(sqlBuilding).ToList<APPOInfo>();
        }

        public List<APPOInfo> getAPPOApproveData(string userName, ApproveSearchBase consdition)
        {
            string statusApprove = "";
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("ib.ADInboxItemDate", consdition.fromDate, consdition.toDate);
            statusApprove = Utils.SelectStatusApprove(consdition.ApprovalStatusCombo, statusApprove);
            var detailStatus = "";
            if (consdition.ApprovalStatusCombo == ApproveStatus.INPROCESS)
            {
                detailStatus = " ('InProgress', 'Approving')";
            }
            var sqlBuilding = String.Format(@"SELECT totalRows = COUNT(*) OVER(), 
tmp.employeeNo, employeeFullName,APPOID,APPONo,APPODate,APPODesc,nccName,APPOAmtTot,
ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID , displayReject
FROM ( SELECT ib.ADInboxItemDate,
                            ISNULL(hre.HREmployeeNo, '') employeeNo,
                            ISNULL(hre.HREmployeeName, '') employeeFullName,
                            details.APPOID,
                            details.APPONo,
                            details.APPODate,
                            details.APPODesc,
                            isnull(aps.APSupplierName, '') nccName,
                            details.APPOAmtTot,
                            details.ApprovalStatusCombo,
                            ib.FK_ADApprovalProcStepID,
                            ib.ADInboxItemID,
                            0 displayReject,
							ROW_NUMBER() OVER(PARTITION BY details.APPOID
                            ORDER BY ib.ADInboxItemDate desc) AS RowNumber
from ADInboxiTEMS ib
inner join APPOs details on details.APPOID = ib.ADInboxItemObjectID
left join APSuppliers aps on aps.APSupplierID = details.FK_APSupplierID and aps.AAStatus = 'Alive'
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
where ib.ADInboxItemTableName = 'APPOs'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2}) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.APPOInfo.FromSqlRaw(sqlBuilding).ToList();
        }

        public List<APPOItemInfo> getDetailAPPOItems(int parrentId)
        {
            var sqlBuilding = String.Format(@"select icp.ICProductNo, icp.ICProductName, apri.APPOItemQty, icu.ICUOMName,apri.APPOItemStkQty,
icu2.ICUOMName ICUOMNameStk, apri.APPOItemArrivalDate, apri.APPOItemUnitPrice, apri.APPOItemAmtTot from APPOItems apri
inner join APPOs apr on apri.FK_APPOID = apr.APPOID AND apri.AAStatus = 'Alive'
inner join ICProducts icp on icp.ICProductID = apri.FK_ICProductID AND icp.AAStatus = 'Alive'
inner join ICUOMs icu on icu.ICUOMID = apri.FK_ICUOMID AND icu.AAStatus = 'Alive'
inner join ICUOMs icu2 on icu2.ICUOMID = apri.FK_ICStkUOMID and icu2.AAStatus = 'Alive'
where apr.APPOID = {0} and apr.AAStatus = 'Alive' order by APPOID", parrentId);
            return _context.APPOItemInfo.FromSqlRaw(sqlBuilding).ToList<APPOItemInfo>();
        }
        #endregion

        #region Voucher payment
        public List<GLVoucherPaymentInfo> getVoucherPaymentMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("details.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
details.GLVoucherID,
details.GLVoucherNo,
details.GLVoucherDate,
details.GLVoucherDesc,
isnull(aps.APSupplierName,'') nccName,
details.GLVoucherFAmtTot,
details.GLVoucherAmtTot,--vn
details.GLOutPmtPayToName,
arc.ARCustomerName,
details.ApprovalStatusCombo,
0 FK_ADApprovalProcStepID,
0 displayReject,
0 ADInboxItemID from GLVouchers details  
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
left join APSuppliers aps on aps.APSupplierID = details.FK_APSupplierID and aps.AAStatus = 'Alive'
left join ARCustomers arc on arc.ARCustomerID = details.FK_ARCustomerID and arc.AAStatus = 'Alive'
WHERE  details.AAStatus = 'Alive' AND details.GLVoucherTypeCombo = 'PmtReq'  {0} AND {1}
ORDER BY details.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" details.GLVoucherID = {0} ", consdition.id) : string.Format(@" (details.AACreatedUser = N'{0}' OR details.FK_HREmployeeID = {1})  ", userName, employeeId), offset, consdition.numberRows);

            return _context.GLVoucherPaymentInfo.FromSqlRaw(sqlBuilding).ToList<GLVoucherPaymentInfo>();
        }

        public List<GLVoucherPaymentInfo> getVoucherPaymentApproveData(string userName, ApproveSearchBase consdition)
        {
            string statusApprove = "";
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("ib.ADInboxItemDate", consdition.fromDate, consdition.toDate);
            statusApprove = Utils.SelectStatusApprove(consdition.ApprovalStatusCombo, statusApprove);
            var detailStatus = "";
            if (consdition.ApprovalStatusCombo == ApproveStatus.INPROCESS)
            {
                detailStatus = " ('InProgress', 'Approving')";
            }
            var sqlBuilding = String.Format(@"SELECT totalRows = COUNT(*) OVER(), 
tmp.employeeNo,
employeeFullName,GLVoucherID,GLVoucherNo,GLVoucherDate,GLVoucherDesc,nccName,GLVoucherFAmtTot,GLVoucherAmtTot,GLOutPmtPayToName,ARCustomerName,
ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID,displayReject
FROM (SELECT ib.ADInboxItemDate,
            ISNULL(hre.HREmployeeNo, '') employeeNo,
            ISNULL(hre.HREmployeeName, '') employeeFullName,
            details.GLVoucherID,
            details.GLVoucherNo,
            details.GLVoucherDate,
            details.GLVoucherDesc,
            isnull(aps.APSupplierName, '') nccName,
            details.GLVoucherFAmtTot,
            details.GLVoucherAmtTot,
            details.GLOutPmtPayToName,
            arc.ARCustomerName,
            details.ApprovalStatusCombo,
            ib.FK_ADApprovalProcStepID,
            ib.ADInboxItemID,
            0 displayReject,
			ROW_NUMBER() OVER(PARTITION BY details.GLVoucherID
            ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM ADInboxiTEMS ib
inner join GLVouchers details on details.GLVoucherID = ib.ADInboxItemObjectID
left join APSuppliers aps on aps.APSupplierID = details.FK_APSupplierID and aps.AAStatus = 'Alive'
left join ARCustomers arc on arc.ARCustomerID = details.FK_ARCustomerID and arc.AAStatus = 'Alive'
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive' 
where ib.ADInboxItemTableName = 'GLVouchers' AND details.GLVoucherTypeCombo = 'PmtReq'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2} ) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.GLVoucherPaymentInfo.FromSqlRaw(sqlBuilding).ToList();
        }

        public List<GLVoucherPaymentItemInfo> getDetailVoucherPaymentItems(int parrentId)
        {
            var sqlBuilding = String.Format(@"select case when RTRIM (LTRIM (glo.GLObjectType)) = 'APSuppliers' then N'Nhà cung cấp' when RTRIM (LTRIM (glo.GLObjectType)) = 'ARCustomers' then 'Khách hàng' when RTRIM (LTRIM (glo.GLObjectType)) = 'HREmployees' then N'Nhân viên' else N'' end as GLObjectTypes,
glo.GLObjectNo,glo.GLObjectName, apri.GLVoucherItemFAmtTot, apri.GLVoucherItemAmtTot, apri.GLVoucherItemDesc  from GLVoucherItems apri
inner join GLVouchers apr on apri.FK_GLVoucherID = apr.GLVoucherID AND apri.AAStatus = 'Alive'
inner join GLObjects glo on glo.GLObjectID = apri.FK_GLObjectID and glo.AAStatus = 'Alive'
where apr.GLVoucherID = {0} and apr.AAStatus = 'Alive' order by apr.GLVoucherID", parrentId);
            return _context.GLVoucherPaymentItemInfo.FromSqlRaw(sqlBuilding).ToList<GLVoucherPaymentItemInfo>();
        }
        #endregion

        #region SO

        public List<ARSOInfo> getSOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("details.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
details.ARSOID,
details.ARSONo,
details.ARSODate,
details.ARSODesc,
details.ARSOFAmtTot,
details.ARSOAmtTot,
arc.ARCustomerName,
details.ApprovalStatusCombo,
0 FK_ADApprovalProcStepID,
0 displayReject,
0 ADInboxItemID from ARSOs details  
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
left join ARCustomers arc on arc.ARCustomerID = details.FK_ARCustomerID and arc.AAStatus = 'Alive'
WHERE  details.AAStatus = 'Alive'  {0} AND {1}
ORDER BY details.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" details.ARSOID = {0} ", consdition.id) : string.Format(@" (details.AACreatedUser = N'{0}' OR details.FK_HREmployeeID = {1})  ", userName, employeeId), offset, consdition.numberRows);


            return _context.ARSOInfo.FromSqlRaw(sqlBuilding).ToList<ARSOInfo>();
        }

        public List<ARSOInfo> getSOApproveData(string userName, ApproveSearchBase consdition)
        {
            string statusApprove = "";
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("ib.ADInboxItemDate", consdition.fromDate, consdition.toDate);
            var detailStatus = "";
            statusApprove = Utils.SelectStatusApprove(consdition.ApprovalStatusCombo, statusApprove);
            if (consdition.ApprovalStatusCombo == ApproveStatus.INPROCESS)
            {
                detailStatus = " ('InProgress', 'Approving')";
            }
            var sqlBuilding = String.Format(@"SELECT totalRows = COUNT(*) OVER(), 
tmp.employeeNo,
employeeFullName,ARSOID,ARSONo,ARSODate,ARSODesc,ARSOFAmtTot,ARSOAmtTot,ARCustomerName,
ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID, displayReject
FROM ( SELECT ib.ADInboxItemDate,
            ISNULL(hre.HREmployeeNo, '') employeeNo,
            ISNULL(hre.HREmployeeName, '') employeeFullName,
            details.ARSOID,
            details.ARSONo,
            details.ARSODate,
            details.ARSODesc,
            details.ARSOFAmtTot,
            details.ARSOAmtTot,
            arc.ARCustomerName,
            details.ApprovalStatusCombo,
            ib.FK_ADApprovalProcStepID,
            ib.ADInboxItemID,
            0 displayReject,
			ROW_NUMBER() OVER(PARTITION BY details.ARSOID
            ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM ADInboxiTEMS ib
inner join ARSOs details on details.ARSOID = ib.ADInboxItemObjectID
left join ARCustomers arc on arc.ARCustomerID = details.FK_ARCustomerID and arc.AAStatus = 'Alive'
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive' 
where ib.ADInboxItemTableName = 'ARSOs'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2} ) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.ARSOInfo.FromSqlRaw(sqlBuilding).ToList();
        }

        public List<ARSOItemInfo> getDetailSOItems(int parrentId)
        {
            var sqlBuilding = String.Format(@"select  icp.ICProductNo, icp.ICProductName , apri.ARSOItemQty, icu.ICUOMName, apri.ARSOItemFUnitPrice, apri.ARSOItemUnitPrice,
apri.ARSOItemFPrice,apri.ARSOItemPrice, apri.ARSOItemFTxAmt,apri.ARSOItemTxAmt, apri.ARSOItemFAmtTot, apri.ARSOItemAmtTot from ARSOItems apri
inner join ARSOs apr on apri.FK_ARSOID = apr.ARSOID AND apri.AAStatus = 'Alive'
inner join ICProducts icp on icp.ICProductID = apri.FK_ICProductID and icp.AAStatus = 'Alive'
left join ICUOMs icu on icu.ICUOMID = apri.FK_ICUOMID
where apr.ARSOID = {0} and apr.AAStatus = 'Alive' order by apr.ARSOID", parrentId);
            return _context.ARSOItemInfo.FromSqlRaw(sqlBuilding).ToList<ARSOItemInfo>();
        }
        #endregion

        public AproveChart collectionData(string userName, int employeeId, int type, string objType)
        {
            var tableName = ApproveType.ApproveTable[objType];
            if (type == 0) // Mydata
            {

                var sqlBuilding = String.Format(@"select 
isnull(sum(CASE WHEN ApprovalStatusCombo = 'InProgress' or  ApprovalStatusCombo = 'Approving' THEN 1 ELSE 0 END),0) Inprocess,
isnull(sum(CASE WHEN ApprovalStatusCombo = 'Approved' THEN 1 ELSE 0 END),0) Approved,
isnull(sum(CASE WHEN ApprovalStatusCombo = 'Rejected' THEN 1 ELSE 0 END),0) Reject,
isnull(sum(CASE WHEN isnull(ApprovalStatusCombo, 'New') = 'New' THEN 1 ELSE 0 END),0) Opens
from {0} where AAStatus = 'Alive' and (AACreatedUser = '{1}' {2})", tableName, userName,
objType.Equals(ApproveType.HR_TRAVEL)?"": string.Format(@" or FK_HREmployeeID = {0} ", employeeId));
                return _context.AproveChart.FromSqlRaw(sqlBuilding).FirstOrDefault();
            } else
            {
                var sqlBuilding = string.Format(@"SELECT 
SUM(CASE WHEN TMP.Approved = 0 THEN 0 ELSE 1 END) Approved,
SUM(CASE WHEN TMP.Reject = 0 THEN 0 ELSE 1 END) Reject,
SUM(CASE WHEN TMP.Inprocess = 0 THEN 0 ELSE 1 END) Inprocess,
SUM(CASE WHEN TMP.Opens = 0 THEN 0 ELSE 1 END) Opens
FROM (select 
isnull(sum(CASE WHEN ib.ADInboxItemAction in ('Approved', 'InProgress', 'Approving') THEN 1 ELSE 0 END),0) Approved,
isnull(sum(CASE WHEN ib.ADInboxItemAction in ('Rejected') THEN 1 ELSE 0 END),0) Reject,
isnull(sum(CASE WHEN isnull(ib.ADInboxItemAction, '') IN ('','Create' ) AND details.ApprovalStatusCombo IN ('InProgress', 'Approving') THEN 1 ELSE 0 END),0) Inprocess,
0 Opens
from ADInboxiTEMS ib
inner join {0} details on details.{1} = ib.ADInboxItemObjectID
WHERE ib.ADInboxItemTableName = '{0}' 
and CHARINDEX('{2};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' {3} GROUP BY details.{1} ) TMP ", tableName, tableName[0..^1] + "ID",
userName, objType.Equals(ApproveType.PAYMENT_REQUEST) ? " AND details.GLVoucherTypeCombo = 'PmtReq' " : "");
                return _context.AproveChart.FromSqlRaw(sqlBuilding).FirstOrDefault();
            }
        }

        public OnePropIntReturn checkNextLevelApprove(int objectId, int aDInboxItemID, string tableName)
        {
            var sqlBuilding = String.Format(@"select count(1) counts
from ADInboxItems where AAStatus = 'Alive' AND ADInboxItemProtocol = 'Approval' AND ADInboxItemObjectID = {0} AND  ADInboxItemTableName = '{1}' 
AND ADInboxItemID > {2} AND ADInboxItemAction = 'Rejected' AND ADInboxItemAction = 'Approved' ", objectId, tableName, aDInboxItemID );
            return _context.OnePropIntReturn.FromSqlRaw(sqlBuilding).FirstOrDefault();
        }

        public TwoPropReturn getLastAproveLevel(int proId)
        {
            var sqlBuilding = String.Format(@"select TOP 1 isnull(AssignUsers,'') dataFirst ,isnull(AssignPositions,'') dataSecond  from ADApprovalProcSteps where FK_ADApprovalProcID = 
(select FK_ADApprovalProcID from ADApprovalProcSteps where ADApprovalProcStepID = {0})
and ADApprovalProcStepApproveToLevel = ADApprovalProcStepLevel and AAStatus = 'Alive' ", proId);
            return _context.TwoPropReturn.FromSqlRaw(sqlBuilding).FirstOrDefault();
        }
    }
}
