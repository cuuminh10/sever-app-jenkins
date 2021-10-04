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
    [Route("chart")]
    public class ChartController : ControllerBase
    {
        private readonly ISimpleService _service;
        private readonly ISystemCommonService _common;

        public ChartController(ISimpleService service, ISystemCommonService common)//, IProductionOrderService producservice)
        {
            _service = service;
            _common = common;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpGet("approve/{type}/{objectType}")]
        public IActionResult GetData(int type, string objectType)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _common.getBasicEmployeeByUserId(userInfo.UserID);
            return Ok(_service.collectionData(userInfo.UserName, empInfo.employeeId, type, objectType));
        }
    }
}
