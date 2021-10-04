using gmc_api.Base;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using gmc_api.DTO.HR;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Repositories
{
    public interface IHREmployeeOffWorkReponsitory : IRepositoriesBase<HREmployeeOffWorks, HREmployeeOffWorks>
    {
        IEnumerable<HREmployeeOffWorkReponse> getMyData(string userName, HREmployeeOffWorkSearch consdition, int employeeId = -1);
        List<HREmployeeOffWorkReponse> getApproveData(string userName, HREmployeeOffWorkSearch consdition);
        HRPeriods GetOjectByDate(DateTime dtDate);
        HREmployeeLRegs GetEmployeeLRegByEmployeeIDAndYear(int iEmployeeID, int iYear);
        HRConfigInfos GetHolidayOfComapny();
    }

    public class HREmployeeOffWorkReponsitory : RepositoriesBaseImpl<HREmployeeOffWorks, HREmployeeOffWorks>, IHREmployeeOffWorkReponsitory
    {
        private readonly GMCContext _context;
        public HREmployeeOffWorkReponsitory(GMCContext context) : base(context, "HREmployeeOffWorks", "HREmployeeOffWorkID")
        {
            _context = context;
        }

        public IEnumerable<HREmployeeOffWorkReponse> getMyData(string userName, HREmployeeOffWorkSearch consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("eow.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
eow.HREmployeeOffWorkID,
eow.HREmployeeOffWorkNo,
eow.FK_HREmployeeLeaveTypeID,
eow.HREmployeeOffWorkFromDate,
eow.HREmployeeOffWorkToDate,
eow.HREmployeeOffWorkTypeCombo,
eow.HREmployeeOffWorkReasonDetail,
eow.HREmployeeOffWorkRegDays,
0 FK_ADApprovalProcStepID,
0 ADInboxItemID,
eow.ApprovalStatusCombo from HREmployeeOffWorks eow 
inner join HREmployees hre on hre.HREmployeeID = eow.FK_HREmployeeID and hre.AAStatus = 'Alive'
inner join ADUsers u on u.ADUserID = hre.FK_ADUserID  and u.AAStatus = 'Alive'
WHERE  eow.AAStatus = 'Alive' {0} AND {1}
ORDER BY eow.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" eow.HREmployeeOffWorkID = {0} ", consdition.id) : string.Format(@" (eow.AACreatedUser = N'{0}' OR eow.FK_HREmployeeID = {1}) ", userName, employeeId), offset, consdition.numberRows);

            return _context.HREmployeeOffWorkReponse.FromSqlRaw(sqlBuilding).ToList<HREmployeeOffWorkReponse>();
        }

        public List<HREmployeeOffWorkReponse> getApproveData(string userName, HREmployeeOffWorkSearch consdition)
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
            var sqlBuilding = String.Format(@"SELECT totalRows = COUNT(*) OVER(), tmp.employeeNo,
employeeFullName,HREmployeeOffWorkID,HREmployeeOffWorkNo,FK_HREmployeeLeaveTypeID,HREmployeeOffWorkFromDate,HREmployeeOffWorkToDate,
HREmployeeOffWorkReasonDetail,HREmployeeOffWorkTypeCombo,HREmployeeOffWorkRegDays, ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID
FROM ( SELECT ib.ADInboxItemDate,
                            ISNULL(hre.HREmployeeNo, '') employeeNo,
                            ISNULL(hre.HREmployeeName, '') employeeFullName,
                            details.HREmployeeOffWorkID,
                            details.HREmployeeOffWorkNo,
                            details.FK_HREmployeeLeaveTypeID,
                            details.HREmployeeOffWorkFromDate,
                            details.HREmployeeOffWorkToDate,
                            details.HREmployeeOffWorkReasonDetail,
                            details.HREmployeeOffWorkTypeCombo,
                            details.HREmployeeOffWorkRegDays,
                            ib.FK_ADApprovalProcStepID,
                            ib.ADInboxItemID,
                            details.ApprovalStatusCombo,
							ROW_NUMBER() OVER(PARTITION BY details.HREmployeeOffWorkID
                            ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM ADInboxiTEMS ib
inner join HREmployeeOffWorks details on details.HREmployeeOffWorkID = ib.ADInboxItemObjectID
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
where ib.ADInboxItemTableName = 'HREmployeeOffWorks'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2} ) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.HREmployeeOffWorkReponse.FromSqlRaw(sqlBuilding).ToList<HREmployeeOffWorkReponse>();
        }

        public HRPeriods GetOjectByDate(DateTime dtDate)
        {
            string sqlBuilding = string.Format(@"SELECT * FROM HRPeriods
                                         WHERE AAStatus='Alive'
                                         AND CONVERT(NVARCHAR(10),HRPeriodFromDate,112) <= '{0}'
                                         AND CONVERT(NVARCHAR(10),HRPeriodToDate,112) >='{0}'", dtDate.ToString("yyyyMMdd"));
            return _context.HRPeriods.FromSqlRaw(sqlBuilding).FirstOrDefault<HRPeriods>();
        }

        public HREmployeeLRegs GetEmployeeLRegByEmployeeIDAndYear(int iEmployeeID, int iYear)
        {
            String sqlBuilding = string.Format("SELECT * FROM [dbo].[HREmployeeLRegs] WHERE [HREmployeeLRegYear]={0}" +
                " AND [AAStatus]='Alive' AND [FK_HREmployeeID] =(SELECT HREmployeeID FROM [dbo].[HREmployees] WHERE" +
                " [AAStatus]='Alive'AND [HREmployeeID]={1} )", iYear, iEmployeeID);
            return _context.HREmployeeLRegs.FromSqlRaw(sqlBuilding).FirstOrDefault<HREmployeeLRegs>();

        }

        public HRConfigInfos GetHolidayOfComapny()
        {
            String sqlBuilding = string.Format("SELECT TOP 1 * FROM [dbo].[HRConfigs] WHERE [AAStatus]='Alive'");
            return _context.HRConfigInfos.FromSqlRaw(sqlBuilding).FirstOrDefault<HRConfigInfos>();
        }
    }
}
