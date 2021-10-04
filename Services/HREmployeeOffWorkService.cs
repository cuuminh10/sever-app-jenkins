using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using gmc_api.DTO.HR;
using gmc_api.Entities;
using gmc_api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Services
{
    public interface IHREmployeeOffWorkService : IServiceGMCBase<HREmployeeOffWorkReponse, HREmployeeOffWorkCreateRequest, HREmployeeOffWorks, HREmployeeOffWorks>
    {
        public HREmployeeOffWorkCreateRequest fillData(HREmployeeOffWorkCreateRequest data, EmployeeBasicInfo empInfo);
        public string validate(HREmployeeOffWorkCreateRequest data, EmployeeBasicInfo empInfo);
        public IEnumerable<HREmployeeOffWorkReponse> getMyData(string userName, HREmployeeOffWorkSearch consdition, int employeeId = -1);
        public IEnumerable<HREmployeeOffWorkReponse> getApproveData(string userName, HREmployeeOffWorkSearch consdition);
        public List<HREmployeeOffWorkReponseCus> addingEdit(IEnumerable<HREmployeeOffWorkReponse> data);
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HREmployeeOffWorkUpdateRequest data);
    }

    public class HREmployeeOffWorkService : ServiceBaseImpl<HREmployeeOffWorkReponse, HREmployeeOffWorkCreateRequest, HREmployeeOffWorks, HREmployeeOffWorks>, IHREmployeeOffWorkService
    {
        private readonly IHREmployeeOffWorkReponsitory _repository;
        private readonly ISimpleReponsitory _simpleReps;
        private readonly IMapper _mapper;

        public HREmployeeOffWorkService(IHREmployeeOffWorkReponsitory repository, IMapper mapper, ISimpleReponsitory simpleReps) : base(repository, mapper)
        {
            _repository = repository;
            _simpleReps = simpleReps;
            _mapper = mapper;
        }

        public List<HREmployeeOffWorkReponseCus> addingEdit(IEnumerable<HREmployeeOffWorkReponse> data)
        {
            var objectMerger = new List<HREmployeeOffWorkReponseCus>();
            foreach (HREmployeeOffWorkReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HREmployeeOffWorkReponseCus>(tmp);
                if (string.IsNullOrEmpty(tmp1.ApprovalStatusCombo))
                    tmp1.isEdit = true;
                else
                    tmp1.isEdit = false;
                objectMerger.Add(tmp1);
            }
            return objectMerger;
        }

        public HREmployeeOffWorkCreateRequest fillData(HREmployeeOffWorkCreateRequest data, EmployeeBasicInfo empInfo)
        {
            if (empInfo != null)
            {
                data.FK_HREmployeeID = empInfo.employeeId;
                data.FK_HRPositionID = empInfo.positionID;
                data.FK_HRSectionID = empInfo.positionID;
            }
            data.HREmployeeOffWorkPeriod = data.HREmployeeOffWorkFromDate.Value.Month;
            data.HREmployeeOffWorkFiscalYear = data.HREmployeeOffWorkFromDate.Value.Year;
            DateTime dt = DateTime.Now;
            data.HREmployeeOffWorkNo = "" + dt.Year + dt.Month + dt.Day + "_" + dt.Hour + dt.Minute + dt.Second;
            return data;
        }
        public SortedDictionary<string, Object> appendObjectUpdate(SortedDictionary<string, Object> rs, HREmployeeOffWorkUpdateRequest data)
        {
            if (data.HREmployeeOffWorkFromDate != Constants.DEFAULT_VALUE_DATETIME)
            {
                rs.Add("HREmployeeOffWorkPeriod", data.HREmployeeOffWorkFromDate.Value.Month);
                rs.Add("HREmployeeOffWorkFiscalYear", data.HREmployeeOffWorkFromDate.Value.Year);
            }
            return rs;
        }
        public IEnumerable<HREmployeeOffWorkReponse> getApproveData(string userName, HREmployeeOffWorkSearch consdition)
        {
            var objectMerger = new List<HREmployeeOffWorkReponseApproveCus>();
            var data = _repository.getApproveData(userName, consdition);
            foreach (HREmployeeOffWorkReponse tmp in data)
            {
                var tmp1 = _mapper.Map<HREmployeeOffWorkReponseApproveCus>(tmp);
                if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED
                    && (tmp.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || tmp.ApprovalStatusCombo == ApproveStatus.INPROCESS))
                {
                    // Check next step conffirm?
                    var isDisplay = _simpleReps.checkNextLevelApprove(tmp.HREmployeeOffWorkID, tmp.ADInboxItemID, "HREmployeeOffWorks");
                    if (isDisplay == null || isDisplay.counts == 0)
                    {
                        tmp1.displayReject = 1;
                    }
                } else if(consdition.ApprovalStatusCombo == ApproveStatus.APROVED
                    && tmp.ApprovalStatusCombo == ApproveStatus.APROVED)
                {
                    // Check user last
                    var lastApprove = _simpleReps.getLastAproveLevel(tmp.FK_ADApprovalProcStepID);
                    if (lastApprove == null )
                    {
                        objectMerger.Add(tmp1);
                        continue;
                    }
                    if(lastApprove.dataFirst.Replace(";", "") == userName)
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
                        for(int i =0; i < positionList.Count; i++)
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

        public IEnumerable<HREmployeeOffWorkReponse> getMyData(string userName, HREmployeeOffWorkSearch consdition, int employeeId = -1)
        {
            return _repository.getMyData(userName, consdition, employeeId);
        }

        public string validate(HREmployeeOffWorkCreateRequest data, EmployeeBasicInfo empInfo)
        {
            if (data.HREmployeeOffWorkFromDate.Value.Date > data.HREmployeeOffWorkToDate.Value.Date)
            {
                return "Ngày nghỉ phép không đúng.";
            }
            int workingDay = getWorkingDayByCompnayHoliday(data.HREmployeeOffWorkFromDate.Value.Date, data.HREmployeeOffWorkToDate.Value.Date);
            if(workingDay != (int)Math.Ceiling(data.HREmployeeOffWorkRegDays) )
            {
                return "Số ngày nghỉ không hợp lệ.";
            }
            var objPeriodInfo = _repository.GetOjectByDate(DateTime.Today);
            if (objPeriodInfo != null)
            {
                if (data.HREmployeeOffWorkFromDate.Value < objPeriodInfo.HRPeriodFromDate.Value || data.HREmployeeOffWorkFromDate.Value > objPeriodInfo.HRPeriodToDate.Value ||
                    data.HREmployeeOffWorkToDate.Value < objPeriodInfo.HRPeriodFromDate.Value || data.HREmployeeOffWorkToDate.Value > objPeriodInfo.HRPeriodToDate.Value)
                {
                    return "Thời gian nghỉ phép nằm ngoài kỳ hiện tại";//, "#Message#", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
            }
            if (_repository.GetEmployeeLRegByEmployeeIDAndYear(empInfo.employeeId, data.HREmployeeOffWorkFromDate.Value.Year) != null ||
                _repository.GetEmployeeLRegByEmployeeIDAndYear(empInfo.employeeId, data.HREmployeeOffWorkToDate.Value.Year) != null)
            {
                return "Đã chốt phép, không được thêm mới báo phép";
            }

            return null;
        }

        private int getWorkingDayByCompnayHoliday(DateTime dtFrom, DateTime dtTo)
        {
            var companyHoliday = _repository.GetHolidayOfComapny();

            int dbSoNgayNghi = 0;
            for (int i = 0; i <= dtTo.Subtract(dtFrom).TotalDays; i++)
            {
                DateTime dt = dtFrom.AddDays(i);
                if (!(companyHoliday.HRConfigOffWork1.Contains(dt.DayOfWeek.ToString()) ||
                    companyHoliday.HRConfigOffWork2.Contains(dt.DayOfWeek.ToString()) ||
                    companyHoliday.HRConfigOffWork3.Contains(dt.DayOfWeek.ToString())))
                {
                    dbSoNgayNghi++;
                }
            }
            return dbSoNgayNghi;
        }
    }
}
