using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.Base.dto;
using gmc_api.DTO.HR;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("voucherPay")]
    public class GLVoucherPaymentController : ControllerBase
    {
        private readonly ISimpleService _service;
        private readonly ISystemCommonService _commonService;

        public GLVoucherPaymentController(ISimpleService service, ISystemCommonService commonService)//, IProductionOrderService producservice)
        {
            _service = service;
            _commonService = commonService;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpGet]
        public IActionResult GetData([FromQuery] ApproveSearchBase searchCondition)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            if (searchCondition.isApprove == 0)
            {
                EmployeeBasicInfo empInfo = _commonService.getBasicEmployeeByUserId(userInfo.UserID);
                var result = _service.getVoucherPaymentMyData(userInfo.UserName, searchCondition, empInfo == null ? -1 : empInfo.employeeId);
                return Ok(result);
            }
            else
            {
                if (searchCondition.ApprovalStatusCombo != ApproveStatus.INPROCESS
                && searchCondition.ApprovalStatusCombo != ApproveStatus.APROVED
                && searchCondition.ApprovalStatusCombo != ApproveStatus.REJECT)
                {
                    return BadRequest(new { message = ApproveStatus.messageValidateSearch() });
                }
                return Ok(_service.getVoucherPaymentApproveData(userInfo.UserName, searchCondition));
            }
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpGet("detail/{id}")]
        public IActionResult GetDetailData(int id)
        {
            return Ok(_service.getDetailVoucherPaymentItems(id));
        }
    }
}
