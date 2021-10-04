using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.dto;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.HR;
using gmc_api.Entities;
using gmc_api.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Services
{
    public interface IADInboxItemService : IServiceGMCBase<ADInboxItemResponse, ADInboxItemInfo, ADInboxItems, ADInboxItemPaging>
    {
        public SQLBuilder buidingSQLClause(string type, UserLoginInfo userInfo, MailBoxSearch searchCondition);

        public List<ADInboxItemResponse> addingEdit(IEnumerable<ADInboxItemResponse> data, List<EmployeeBasicInfo> empInfo);
    }

    public class ADInboxItemService : ServiceBaseImpl<ADInboxItemResponse, ADInboxItemInfo, ADInboxItems, ADInboxItemPaging>, IADInboxItemService
    {
        private readonly IADInboxItemReponsitory _repository;
        private readonly IMapper _mapper;

        public ADInboxItemService(IADInboxItemReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ADInboxItemResponse> addingEdit(IEnumerable<ADInboxItemResponse> data, List<EmployeeBasicInfo> empInfo)
        {
            var objectMerger = new List<ADInboxItemResponse>();
            foreach (ADInboxItemResponse tmp in data)
            {
                var emp = empInfo.Where(s => s.employeeId == tmp.FK_HRFromEmployeeID).FirstOrDefault();
                if (emp != null && emp.employeeName != null)
                    tmp.employeeFullName = emp.employeeName;
                else
                {
                    emp = empInfo.Where(s => s.userId == tmp.FK_ADFromUserID).FirstOrDefault();
                    if (emp != null && emp.employeeName != null)
                        tmp.employeeFullName = emp.employeeName;
                    else
                        tmp.employeeFullName = "";
                }
                objectMerger.Add(tmp);
            }
            return objectMerger;
        }

        public SQLBuilder buidingSQLClause(string type, UserLoginInfo userInfo, MailBoxSearch searchCondition)
        {
            var result = new SQLBuilder
            {
                paging = new Paging()
            };
            result.paging.pageNo = searchCondition.pageNo;
            result.paging.numberRows = searchCondition.numberRows;
            result.orderByCondition = " ADInboxItemDate DESC";
            result.sqlCondition = string.Format(" AND (CHARINDEX('{0};', ADMailToUsers ) > 0 OR CHARINDEX('{0};', ADMailCCUsers) > 0)", userInfo.UserName);
            var interval = Utils.buildConditionFromDateToDate("ADInboxItemDate", searchCondition.fromDate, searchCondition.toDate);
            result.sqlCondition += interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "";
            result.sqlCondition += string.IsNullOrEmpty(searchCondition.objectType) ? "" : string.Format(" AND ADInboxItemDocType = N'{0}' ", searchCondition.objectType);
            return result;
        }
    }
}
