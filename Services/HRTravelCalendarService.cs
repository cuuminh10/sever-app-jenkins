using AutoMapper;
using gmc_api.DTO.HR;
using gmc_api.Entities;
using gmc_api.Repositories;
using System;
using System.Collections.Generic;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.Base.Helpers;
using static gmc_api.Base.Helpers.Constants;
using System.Linq;

namespace gmc_api.Services
{
    public interface IHRTravelCalendarService : IServiceGMCBase<HRTravelCalendarReponse, HRTravelCalendarCreateRequest, HRTravelCalendars, HRTravelCalendars>
    {
        public HRTravelCalendarCreateRequest fillData(HRTravelCalendarCreateRequest data, string no, EmployeeBasicInfo empInfo);
        public IEnumerable<HRTravelCalendarReponse> getMyData(string userName, HRTravelCalendarSearch consdition);
        public IEnumerable<HRTravelCalendarReponseApproveCus> getApproveData(string userName, HRTravelCalendarSearch consdition);
        public List<HRTravelCalendarReponseCus> addingEdit(IEnumerable<HRTravelCalendarReponse> data);
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HRTravelCalendarUpdateRequest data);
    }

    public class HRTravelCalendarService : ServiceBaseImpl<HRTravelCalendarReponse, HRTravelCalendarCreateRequest, HRTravelCalendars, HRTravelCalendars>, IHRTravelCalendarService
    {
        private readonly IHRTravelCalendarReponsitory _repository;
        private readonly ISimpleReponsitory _simpleReps;
        private readonly IMapper _mapper;

        public HRTravelCalendarService(IHRTravelCalendarReponsitory repository, IMapper mapper, ISimpleReponsitory simpleReps) : base(repository, mapper)
        {
            _repository = repository;
            _simpleReps = simpleReps;
            _mapper = mapper;
        }

        public List<HRTravelCalendarReponseCus> addingEdit(IEnumerable<HRTravelCalendarReponse> data)
        {
            var objectMerger = new List<HRTravelCalendarReponseCus>();
            foreach (HRTravelCalendarReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HRTravelCalendarReponseCus>(tmp);
                if (string.IsNullOrEmpty(tmp1.ApprovalStatusCombo))
                    tmp1.isEdit = true;
                else
                    tmp1.isEdit = false;
                objectMerger.Add(tmp1);
            }
            return objectMerger;
        }

        public HRTravelCalendarCreateRequest fillData(HRTravelCalendarCreateRequest data, string no, EmployeeBasicInfo empInfo)
        {
            if (empInfo != null)
            {
                data.FK_HRDepartmentID = empInfo.departmentID;
            }
            data.HRTravelCalendarNo = no;
            data.HRTravelCalendarName = "PTC-" + empInfo.employeeName;
            var tmpDate = data.HRTravelCalendarToDate - data.HRTravelCalendarFromDate;
            data.HRTravelCalendarRealDay = (int)tmpDate.Value.TotalDays;
            if (tmpDate.Value.TotalSeconds > 0)
                data.HRTravelCalendarRealDay++;
            data.HRTravelCalendarTempDay = data.HRTravelCalendarRealDay;
            return data;
        }
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HRTravelCalendarUpdateRequest data)
        {
            if (data.HRTravelCalendarToDate != Constants.DEFAULT_VALUE_DATETIME
                && data.HRTravelCalendarFromDate != Constants.DEFAULT_VALUE_DATETIME)
            {
                var realDay = data.HRTravelCalendarToDate - data.HRTravelCalendarFromDate;
                var addDay = realDay.Value.TotalDays;
                if (realDay.Value.TotalSeconds > 0)
                {
                    addDay++;
                }
                rs.Add("HRTravelCalendarRealDay", addDay);
                rs.Add("HRTravelCalendarTempDay", addDay);
            }
            else
            {
                if (data.HRTravelCalendarToDate != Constants.DEFAULT_VALUE_DATETIME)
                {
                    var objectOverTime = _repository.GetObjectById(data.HRTravelCalendarID);
                    var realDay = objectOverTime.HRTravelCalendarToDate - data.HRTravelCalendarFromDate;
                    var addDay = realDay.Value.TotalDays;
                    if (realDay.Value.TotalSeconds > 0)
                    {
                        addDay++;
                    }
                    rs.Add("HRTravelCalendarRealDay", addDay);
                    rs.Add("HRTravelCalendarTempDay", addDay);
                }
                else if (data.HRTravelCalendarFromDate != Constants.DEFAULT_VALUE_DATETIME)
                {
                    var objectOverTime = _repository.GetObjectById(data.HRTravelCalendarID);
                    var realDay = data.HRTravelCalendarToDate - objectOverTime.HRTravelCalendarFromDate;
                    var addDay = realDay.Value.TotalDays;
                    if (realDay.Value.TotalSeconds > 0)
                    {
                        addDay++;
                    }
                    rs.Add("HRTravelCalendarRealDay", addDay);
                    rs.Add("HRTravelCalendarTempDay", addDay);
                }
            }
            return rs;
        }
        public IEnumerable<HRTravelCalendarReponseApproveCus> getApproveData(string userName, HRTravelCalendarSearch consdition)
        {
            var objectMerger = new List<HRTravelCalendarReponseApproveCus>();
            var data = _repository.getApproveData(userName, consdition);
            foreach (HRTravelCalendarReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HRTravelCalendarReponseApproveCus>(tmp);
                if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED
                    && (tmp.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || tmp.ApprovalStatusCombo == ApproveStatus.INPROCESS))
                {
                    // Check next step conffirm?
                    var isDisplay = _simpleReps.checkNextLevelApprove(tmp.HRTravelCalendarID, tmp.ADInboxItemID, "HRTravelCalendars");
                    if (isDisplay == null || isDisplay.counts == 0)
                    {
                        tmp1.displayReject = 1;
                    }
                }
                else if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED
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

        public IEnumerable<HRTravelCalendarReponse> getMyData(string userName, HRTravelCalendarSearch consdition)
        {
            return _repository.getMyData(userName, consdition);
        }
    }
}
