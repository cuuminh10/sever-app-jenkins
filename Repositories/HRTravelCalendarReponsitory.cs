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
    public interface IHRTravelCalendarReponsitory : IRepositoriesBase<HRTravelCalendars, HRTravelCalendars>
    {
        IEnumerable<HRTravelCalendarReponse> getMyData(string userName, HRTravelCalendarSearch consdition);
        List<HRTravelCalendarReponse> getApproveData(string userName, HRTravelCalendarSearch consdition);
    }

    public class HRTravelCalendarReponsitory : RepositoriesBaseImpl<HRTravelCalendars, HRTravelCalendars>, IHRTravelCalendarReponsitory
    {
        private readonly GMCContext _context;
        public HRTravelCalendarReponsitory(GMCContext context) : base(context, "HRTravelCalendars", "HRTravelCalendarID")
        {
            _context = context;
        }

        public IEnumerable<HRTravelCalendarReponse> getMyData(string userName, HRTravelCalendarSearch consdition)
        {
            var offset = (consdition.pageNo - 1) * consdition.numberRows;
            var interval = Utils.buildConditionFromDateToDate("detail.AACreatedDate", consdition.fromDate, consdition.toDate);
            var sqlBuilding = String.Format(@"select totalRows = COUNT(*) OVER(),
hre.HREmployeeNo employeeNo,
hre.HREmployeeName employeeFullName,
detail.HRTravelCalendarID,
detail.HRTravelCalendarNo,
detail.HRTravelCalendarName,
detail.HRTravelCalendarDate,
detail.HRTravelCalendarDesc,
detail.HRTravelCalendarFromDate,
detail.HRTravelCalendarToDate,
detail.FK_HRTravelTypeID,
0 FK_ADApprovalProcStepID,
0 ADInboxItemID,
detail.ApprovalStatusCombo,
detail.HRTravelCalendarRealDay,
detail.FK_HRProvinceID,
detail.HRTravelCalendarSundayCheck,
detail.HRTravelCalendarNoCalCheck from HRTravelCalendars detail  
inner join ADUsers u on u.ADUserName = detail.AACreatedUser  and u.AAStatus = 'Alive'
inner join HREmployees hre on hre.FK_ADUserID = U.ADUserID and hre.AAStatus = 'Alive'
WHERE  detail.AAStatus = 'Alive' {0} AND {1}
ORDER BY detail.AACreatedDate DESC OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "",
consdition.id != 0 ? string.Format(" detail.HRTravelCalendarID = {0} ", consdition.id) : string.Format(@" detail.AACreatedUser = N'{0}' ", userName), offset, consdition.numberRows);
            return _context.HRTravelCalendarReponse.FromSqlRaw(sqlBuilding).ToList<HRTravelCalendarReponse>();
        }

        public List<HRTravelCalendarReponse> getApproveData(string userName, HRTravelCalendarSearch consdition)
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
tmp.employeeNo,HRTravelCalendarSundayCheck,HRTravelCalendarNoCalCheck,
employeeFullName,HRTravelCalendarID,HRTravelCalendarNo,HRTravelCalendarName,HRTravelCalendarDate,HRTravelCalendarDesc,HRTravelCalendarFromDate,
HRTravelCalendarToDate,FK_HRTravelTypeID,ApprovalStatusCombo, FK_ADApprovalProcStepID, ADInboxItemID,HRTravelCalendarRealDay,FK_HRProvinceID
FROM ( SELECT ib.ADInboxItemDate,
        hre.HREmployeeNo employeeNo,
        hre.HREmployeeName employeeFullName,
        detail.HRTravelCalendarID,
        detail.HRTravelCalendarNo,
        detail.HRTravelCalendarName,
        detail.HRTravelCalendarDate,
        detail.HRTravelCalendarDesc,
        detail.HRTravelCalendarFromDate,
        detail.HRTravelCalendarToDate,
        detail.FK_HRTravelTypeID,
        ib.FK_ADApprovalProcStepID,
        ib.ADInboxItemID,
        detail.ApprovalStatusCombo,
        detail.HRTravelCalendarRealDay,
        detail.FK_HRProvinceID,
        detail.HRTravelCalendarSundayCheck,
        detail.HRTravelCalendarNoCalCheck,
		ROW_NUMBER() OVER(PARTITION BY detail.HRTravelCalendarID
        ORDER BY ib.ADInboxItemDate desc) AS RowNumber
FROM ADInboxiTEMS ib
inner join HRTravelCalendars detail on detail.HRTravelCalendarID = ib.ADInboxItemObjectID
inner join ADUsers u on u.ADUserName = detail.AACreatedUser  and u.AAStatus = 'Alive'
inner join HREmployees hre on hre.FK_ADUserID = U.ADUserID and hre.AAStatus = 'Alive'
where ib.ADInboxItemTableName = 'HRTravelCalendars' and CHARINDEX('{0};', ADMailToUsers) > 0 and ib.ADInboxItemProtocol = 'Approval' and isnull(ADInboxItemAction,'') IN ({1}) {5}
{2} ) tmp where tmp.RowNumber = 1 ORDER BY tmp.ADInboxItemDate desc OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY "
, userName, statusApprove, interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "", offset, consdition.numberRows,
detailStatus != "" ? string.Format(@" AND isnull(detail.ApprovalStatusCombo, '') IN {0} ", detailStatus) : "");
            return _context.HRTravelCalendarReponse.FromSqlRaw(sqlBuilding).ToList<HRTravelCalendarReponse>();
        }
    }
}
