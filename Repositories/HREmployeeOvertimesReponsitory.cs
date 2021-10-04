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
    public interface IHREmployeeOvertimesReponsitory : IRepositoriesBase<HREmployeeOvertimes, HREmployeeOvertimes>
    {
        IEnumerable<HREmployeeOvertimeReponse> getMyData(string userName, HREmployeeOvertimeSearch consdition, int employeeId = -1);
        List<HREmployeeOvertimeReponse> getApproveData(string userName, HREmployeeOvertimeSearch consdition);
    }

    public class HREmployeeOvertimesReponsitory : RepositoriesBaseImpl<HREmployeeOvertimes, HREmployeeOvertimes>, IHREmployeeOvertimesReponsitory
    {
        private readonly GMCContext _context;
        public HREmployeeOvertimesReponsitory(GMCContext context) : base(context, "HREmployeeOvertimes", "HREmployeeOvertimeID")
        {
            _context = context;
        }

        public IEnumerable<HREmployeeOvertimeReponse> getMyData(string userName, HREmployeeOvertimeSearch consdition, int employeeId = -1)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("eow.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(), hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
eow.HREmployeeOvertimeID,
eow.HREmployeeOvertimeNo,
eow.HREmployeeOvertimeFromDate,
eow.HREmployeeOvertimeToDate,
eow.FK_HROvertimeRateID,
eow.FK_HRShiftID,
eow.HREmployeeOvertimeReasonDetail,
eow.ApprovalStatusCombo,
eow.HREmployeeOvertimeCTCheck,
eow.HREmployeeOvertimeBreakHour,
0 FK_ADApprovalProcStepID,
0 ADInboxItemID from HREmployeeOvertimes eow  
inner join HREmployees hre on hre.HREmployeeID = eow.FK_HREmployeeID and hre.AAStatus = 'Alive'
inner join ADUsers u on u.ADUserID = hre.FK_ADUserID  and u.AAStatus = 'Alive'
WHERE  eow.AAStatus = 'Alive' {0} AND {1}
ORDER BY eow.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" eow.HREmployeeOvertimeID = {0} ", consdition.id) : string.Format(@" (eow.AACreatedUser = N'{0}' OR eow.FK_HREmployeeID = {1}) ", userName, employeeId), offset, consdition.numberRows);

            return _context.HREmployeeOvertimeReponse.FromSqlRaw(sqlBuilding).ToList<HREmployeeOvertimeReponse>();
        }

        public List<HREmployeeOvertimeReponse> getApproveData(string userName, HREmployeeOvertimeSearch consdition)
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
employeeFullName,HREmployeeOvertimeID,HREmployeeOvertimeNo,HREmployeeOvertimeFromDate,HREmployeeOvertimeToDate,FK_HROvertimeRateID,FK_HRShiftID,
HREmployeeOvertimeReasonDetail,ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID,HREmployeeOvertimeCTCheck,HREmployeeOvertimeBreakHour
FROM ( SELECT ib.ADInboxItemDate,
        ISNULL(hre.HREmployeeNo, '') employeeNo,
        ISNULL(hre.HREmployeeName, '') employeeFullName,
        details.HREmployeeOvertimeID,
        details.HREmployeeOvertimeNo,
        details.HREmployeeOvertimeFromDate,
        details.HREmployeeOvertimeToDate,
        details.FK_HROvertimeRateID,
        details.FK_HRShiftID,
        details.HREmployeeOvertimeReasonDetail,
        details.ApprovalStatusCombo,
        details.HREmployeeOvertimeCTCheck,
        details.HREmployeeOvertimeBreakHour,
        ib.FK_ADApprovalProcStepID,
        ib.ADInboxItemID,
		ROW_NUMBER() OVER(PARTITION BY details.HREmployeeOvertimeID
        ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM ADInboxiTEMS ib
inner join HREmployeeOvertimes details on details.HREmployeeOvertimeID = ib.ADInboxItemObjectID
left join HREmployees hre on hre.HREmployeeID = details.FK_HREmployeeID and hre.AAStatus = 'Alive'
where ib.ADInboxItemTableName = 'HREmployeeOvertimes'
and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2}) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(details.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.HREmployeeOvertimeReponse.FromSqlRaw(sqlBuilding).ToList<HREmployeeOvertimeReponse>();
        }
    }
}
