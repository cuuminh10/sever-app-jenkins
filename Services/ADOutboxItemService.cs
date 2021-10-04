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
    public interface IADOutboxItemService : IServiceGMCBase<ADOutboxItemResponse, ADOuboxItemInfo, ADOutboxItems, ADOutboxItemPaging>
    {
        public SQLBuilder buidingSQLClause(string type, UserLoginInfo userInfo, MailBoxSearch searchCondition);
        public List<ADOutboxItemResponse> addingEdit(IEnumerable<ADOutboxItemResponse> data, List<EmployeeBasicInfo> empInfo);
    }

    public class ADOutboxItemService : ServiceBaseImpl<ADOutboxItemResponse, ADOuboxItemInfo, ADOutboxItems, ADOutboxItemPaging>, IADOutboxItemService
    {
        private readonly IADOutboxItemReponsitory _repository;
        private readonly IMapper _mapper;

        public ADOutboxItemService(IADOutboxItemReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ADOutboxItemResponse> addingEdit(IEnumerable<ADOutboxItemResponse> data, List<EmployeeBasicInfo> empInfo)
        {
            var objectMerger = new List<ADOutboxItemResponse>();
            foreach (ADOutboxItemResponse tmp in data)
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
            result.orderByCondition = " ADOutboxItemDate DESC";
            result.sqlCondition = string.Format(@" AND FK_ADFromUserID = {0}", userInfo.UserID);
            var interval = Utils.buildConditionFromDateToDate("ADOutboxItemDate", searchCondition.fromDate, searchCondition.toDate);
            result.sqlCondition += interval.Trim().Length > 0 ? string.Format(" AND {0}", interval) : "";
            result.sqlCondition += string.IsNullOrEmpty(searchCondition.objectType) ? "" : string.Format(" AND ADOutboxItemDocType = N'{0}' ", searchCondition.objectType);
            return result;
        }
    }
}
