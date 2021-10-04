using gmc_api.DTO.HR;
using gmc_api.Services;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using gmc_api.Base.dto;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("offWork")]
    public class HREmployeeOffWorkController : ControllerBase
    {
        private readonly IHREmployeeOffWorkService _service;
        private readonly ISystemCommonService _commonService;

        public HREmployeeOffWorkController(IHREmployeeOffWorkService service, ISystemCommonService commonService)
        {
            _service = service;
            _commonService = commonService;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpPost]
        public IActionResult Create(HREmployeeOffWorkCreateRequest data)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
            data = _service.fillData(data, empInfo);
           // var errors = _service.validate(data, empInfo);
         //   if (errors != null)
        //    {
        //        return BadRequest(new { message = errors });
       //     }
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
        public IActionResult Update(int id, HREmployeeOffWorkUpdateRequest data)
        {
            data.HREmployeeOffWorkID = id;
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
            var listProp = Utils.getPropertiesForUpdate(data);
            listProp.Add("ApprovalStatusCombo", "New");
           // var oldData = _service.GetObjectCreateMapper(id);
           // var errors = _service.validate((HREmployeeOffWorkCreateRequest)Utils.OveriteProperties(oldData, listProp), empInfo);
         //   if(errors != null)
         //   {
        //        return BadRequest(new { message = errors });
         //   }
            var updateCount = _service.UpdateObject(_service.appendObjectUpdate(listProp, data));
            if (updateCount == 0)
                return BadRequest(new { message = "No record updated, please check again input data" });
            return Ok(Utils.makeDedaultOrEmptyToNull(data, "HREmployeeOffWorkID"));
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpGet]
        public IActionResult GetData([FromQuery] HREmployeeOffWorkSearch searchCondition)
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
