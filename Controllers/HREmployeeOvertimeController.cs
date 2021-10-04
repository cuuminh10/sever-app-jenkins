using gmc_api.Base.dto;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.DTO.HR;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("OT")]
    public class HREmployeeOvertimeController : ControllerBase
    {
        private readonly IHREmployeeOvertimeService _service;
        private readonly ISystemCommonService _commonService;

        public HREmployeeOvertimeController(IHREmployeeOvertimeService service, ISystemCommonService commonService)
        {
            _service = service;
            _commonService = commonService;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpPost]
        public IActionResult Create(HREmployeeOvertimeCreateRequest data)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
            data = _service.fillData(data, empInfo);
            var fav = _service.CreateObject(data, userInfo);
            if (fav == null)
                return BadRequest(new { message = "Data input not correct" });
            fav.employeeFullName = empInfo != null ? empInfo.employeeName : "";
            fav.employeeNo = empInfo != null ? empInfo.employeeNo : "";
            return Ok(fav);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public IActionResult Update(int id, HREmployeeOvertimeUpdateRequest data)
        {
            data.HREmployeeOvertimeID = id;
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
        public IActionResult GetData([FromQuery] HREmployeeOvertimeSearch searchCondition)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            if (searchCondition.isApprove == 0)
            {
                EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
                var result = _service.getMyData(userInfo.UserName, searchCondition, empInfo == null ? -1 : empInfo.employeeId);
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
