using gmc_api.DTO.HR;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using gmc_api.Base.dto;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("travel")]
    public class HRTravelCalendarController : ControllerBase
    {
        private readonly IHRTravelCalendarService _service;
        private readonly IHRTravelCalendarItemService _itemService;
        private readonly ISystemCommonService _commonService;

        public HRTravelCalendarController(IHRTravelCalendarService service, ISystemCommonService commonService, IHRTravelCalendarItemService itemService)
        {
            _service = service;
            _commonService = commonService;
            _itemService = itemService;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpPost]
        public IActionResult Create(HRTravelCalendarCreateRequest data)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
            if (empInfo == null)
            {
                return BadRequest(new { message = "Employee not exist in system!!!" });
            }
            var no = _commonService.GetGeneralNoItem(data, TableModule.TravelCalendar);
            data = _service.fillData(data, no, empInfo);
            var fav = _service.CreateObject(data, userInfo);
            if (fav == null)
                return BadRequest(new { message = "Data input not correct" });
            fav.employeeFullName = empInfo != null ? empInfo.employeeName : "";
            fav.employeeNo = empInfo != null ? empInfo.employeeNo : "";
            HRTravelCalendarItemCreateRequest travelItem = new()
            {
                FK_HREmployeeID = empInfo.employeeId,
                FK_HRTravelCalendarID = fav.HRTravelCalendarID
                // HRTravelCalendarNote = data.HRTravelCalendarDesc
            };
            _itemService.CreateObject(travelItem);
            return Ok(fav);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public IActionResult Update(int id, HRTravelCalendarUpdateRequest data)
        {
            data.HRTravelCalendarID = id;
            var listProp = Utils.getPropertiesForUpdate(data);
            listProp.Add("ApprovalStatusCombo", "New");
            var updateCount = _service.UpdateObject(_service.appendObjectUpdate(listProp, data));
            if (updateCount == 0)
                return BadRequest(new { message = "No record updated, please check again input data" });
            return Ok(Utils.makeDedaultOrEmptyToNull(data, "HREmployeeOvertimeID"));
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpGet]
        public IActionResult GetData([FromQuery] HRTravelCalendarSearch searchCondition)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            if (searchCondition.isApprove == 0)
            {
                EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
                var result = _service.getMyData(userInfo.UserName, searchCondition);
                return Ok(_service.addingEdit(result));
            }
            else
            {
                if (searchCondition.ApprovalStatusCombo != ApproveStatus.INPROCESS
                && searchCondition.ApprovalStatusCombo != ApproveStatus.APROVED
                && searchCondition.ApprovalStatusCombo != ApproveStatus.REJECT)
                {
                    return BadRequest(new { message = ApproveStatus.messageValidateSearch() });
                }
                return Ok(_service.getApproveData(userInfo.UserName, searchCondition));
            }
        }
    }
}
