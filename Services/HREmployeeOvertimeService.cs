using AutoMapper;
using gmc_api.DTO.HR;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.Entities;
using gmc_api.Repositories;
using System;
using System.Collections.Generic;
using gmc_api.Base.Helpers;
using static gmc_api.Base.Helpers.Constants;
using System.Linq;

namespace gmc_api.Services
{
    public interface IHREmployeeOvertimeService : IServiceGMCBase<HREmployeeOvertimeReponse, HREmployeeOvertimeCreateRequest, HREmployeeOvertimes, HREmployeeOvertimes>
    {
        public HREmployeeOvertimeCreateRequest fillData(HREmployeeOvertimeCreateRequest data, EmployeeBasicInfo empInfo);
        public IEnumerable<HREmployeeOvertimeReponse> getMyData(string userName, HREmployeeOvertimeSearch consdition, int employeeId = -1);
        public IEnumerable<HREmployeeOvertimeReponseApproveCus> getApproveData(string userName, HREmployeeOvertimeSearch consdition);
        public List<HREmployeeOvertimeReponseCus> addingEdit(IEnumerable<HREmployeeOvertimeReponse> data);
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HREmployeeOvertimeUpdateRequest data);
    }

    public class HREmployeeOvertimeService : ServiceBaseImpl<HREmployeeOvertimeReponse, HREmployeeOvertimeCreateRequest, HREmployeeOvertimes, HREmployeeOvertimes>, IHREmployeeOvertimeService
    {
        private readonly IHREmployeeOvertimesReponsitory _repository;
        private readonly ISimpleReponsitory _simpleReps;
        private readonly IMapper _mapper;

        public HREmployeeOvertimeService(IHREmployeeOvertimesReponsitory repository, IMapper mapper, ISimpleReponsitory simpleReps) : base(repository, mapper)
        {
            _repository = repository;
            _simpleReps = simpleReps;
            _mapper = mapper;
        }

        public List<HREmployeeOvertimeReponseCus> addingEdit(IEnumerable<HREmployeeOvertimeReponse> data)
        {
            var objectMerger = new List<HREmployeeOvertimeReponseCus>();
            foreach (HREmployeeOvertimeReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HREmployeeOvertimeReponseCus>(tmp);
                if (string.IsNullOrEmpty(tmp1.ApprovalStatusCombo))
                    tmp1.isEdit = true;
                else
                    tmp1.isEdit = false;
                objectMerger.Add(tmp1);
            }
            return objectMerger;
        }

        public HREmployeeOvertimeCreateRequest fillData(HREmployeeOvertimeCreateRequest data, EmployeeBasicInfo empInfo)
        {
            if (empInfo != null)
            {
                data.FK_HREmployeeID = empInfo.employeeId;
            }

            data.HREmployeeOvertimePeriod = data.HREmployeeOvertimeFromDate.Value.Month;
            data.HREmployeeOvertimeFiscalYear = data.HREmployeeOvertimeFromDate.Value.Year;
            DateTime dt = DateTime.Now;
            data.HREmployeeOvertimeNo = "" + dt.Year + dt.Month + dt.Day + "_" + dt.Hour + dt.Minute + dt.Second;
            data.HREmployeeOvertimeHour = (decimal)(data.HREmployeeOvertimeToDate - data.HREmployeeOvertimeFromDate).Value.TotalHours - data.HREmployeeOvertimeBreakHour;
            if (data.HREmployeeOvertimeHour > 8)
                data.HREmployeeOvertimeOverDayCheck = true;
            return data;
        }
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HREmployeeOvertimeUpdateRequest data)
        {
            bool callAgain = false;
            var objectOverTime = _repository.GetObjectById(data.HREmployeeOvertimeID);
            if (data.HREmployeeOvertimeFromDate != Constants.DEFAULT_VALUE_DATETIME)
            {
                rs.Add("HREmployeeOvertimePeriod", data.HREmployeeOvertimeFromDate.Value.Month);
                rs.Add("HREmployeeOvertimeFiscalYear", data.HREmployeeOvertimeFromDate.Value.Year);
                objectOverTime.HREmployeeOvertimeFromDate = data.HREmployeeOvertimeFromDate;
                callAgain = true;
            }
            if (data.HREmployeeOvertimeToDate != Constants.DEFAULT_VALUE_DATETIME)
            {
                objectOverTime.HREmployeeOvertimeToDate = data.HREmployeeOvertimeToDate;
                callAgain = true;
            }
            if (data.HREmployeeOvertimeBreakHour != Constants.DEFAULT_VALUE_DECIMAL)
            {
                objectOverTime.HREmployeeOvertimeBreakHour = data.HREmployeeOvertimeBreakHour;
                callAgain = true;
            }
            if (callAgain)
            {
                decimal overHour = (decimal)(objectOverTime.HREmployeeOvertimeToDate - objectOverTime.HREmployeeOvertimeFromDate).Value.TotalHours - objectOverTime.HREmployeeOvertimeBreakHour;
                rs.Add("HREmployeeOvertimeHour", overHour);
                if (overHour > 8)
                    rs.Add("HREmployeeOvertimeOverDayCheck", true);
                else
                    rs.Add("HREmployeeOvertimeOverDayCheck", false);
            }
            return rs;
        }
        public IEnumerable<HREmployeeOvertimeReponseApproveCus> getApproveData(string userName, HREmployeeOvertimeSearch consdition)
        {
            var objectMerger = new List<HREmployeeOvertimeReponseApproveCus>();
            var data = _repository.getApproveData(userName, consdition);
            foreach (HREmployeeOvertimeReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HREmployeeOvertimeReponseApproveCus>(tmp);
                if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED
                    && (tmp.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || tmp.ApprovalStatusCombo == ApproveStatus.INPROCESS))
                {
                    // Check next step conffirm?
                    var isDisplay = _simpleReps.checkNextLevelApprove(tmp.HREmployeeOvertimeID, tmp.ADInboxItemID, "HREmployeeOvertimes");
                    if (isDisplay == null || isDisplay.counts == 0)
                    {
                        tmp1.displayReject = 1;
                    }
                } else if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED
                   && tmp.ApprovalStatusCombo == ApproveStatus.APROVED)
                {
                    // Check user last
                    var lastApprove = _simpleReps.getLastAproveLevel(tmp.FK_ADApprovalProcStepID);
                    if (lastApprove == null)
                    {
                        objectMerger.Add(tmp1);
                        continue;
                    }
                    if (lastApprove.dataFirst.Replace(";", "") == userName)
                    {
                        tmp1.displayReject = 1;
                        objectMerger.Add(tmp1);
                        continue;
                    }
                    if (lastApprove.dataSecond != null && lastApprove.dataSecond.Trim().Length > 0)
                    {
                        var tmpUserPosition = lastApprove.dataSecond.Split(";").ToList<string>();
                        tmpUserPosition = tmpUserPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                        var positionList = _simpleReps.getUserByPosition(tmpUserPosition);
                        if (positionList == null || positionList.Count == 0)
                        {
                            objectMerger.Add(tmp1);
                            continue;
                        }
                        for (int i = 0; i < positionList.Count; i++)
                        {
                            if (positionList[i].dataReturn == userName)
                            {
                                tmp1.displayReject = 1;
                                break;
                            }
                        }

                    }
                }
                objectMerger.Add(tmp1);
            }
            return objectMerger;
        }

        public IEnumerable<HREmployeeOvertimeReponse> getMyData(string userName, HREmployeeOvertimeSearch consdition, int employeeId = -1)
        {
            return _repository.getMyData(userName, consdition, employeeId);
        }
    }
}
