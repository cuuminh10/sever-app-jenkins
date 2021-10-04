using gmc_api.Base.dto;
using gmc_api.Base.Helpers;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.HR;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController()]
    [Route("mailBox")]
    public class MailBoxController : ControllerBase
    {
        private readonly IADInboxItemService _service;
        private readonly IADOutboxItemService _outBoxService;
        private readonly ISystemCommonService _commonService;
        private readonly IHREmployeeOffWorkService _employeeService;
        private readonly IHREmployeeOvertimeService _otService;
        private readonly IHRTravelCalendarService _travelService;
        private readonly ISimpleService _simpleService;

        public MailBoxController(IADInboxItemService service, ISystemCommonService commonService, IADOutboxItemService outBoxService,
            IHREmployeeOffWorkService employeeService, IHREmployeeOvertimeService otService, IHRTravelCalendarService travelService,
            ISimpleService simpleService)
        {
            _service = service;
            _commonService = commonService;
            _outBoxService = outBoxService;
            _employeeService = employeeService;
            _otService = otService;
            _travelService = travelService;
            _simpleService = simpleService;
        }

        [Authorize]
        [HttpGet("{type}")]
        public IActionResult GetAll(string type, [FromQuery] MailBoxSearch searchCondition)
        {
            if (!MailBoxType.isIn(type) || !(string.IsNullOrEmpty(searchCondition.objectType)
                || ApproveType.isIn(searchCondition.objectType)))
            {
                return BadRequest(new { message = "Data input not correct!!!" });
            }
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            if (type == MailBoxType.INBOX)
            {
                var sqlBuilder = _service.buidingSQLClause(type, userInfo, searchCondition);

                List<ADInboxItemResponse> data = (List<ADInboxItemResponse>)_service.GetPagingData(sqlBuilder.sqlCondition,
                    sqlBuilder.orderByCondition, sqlBuilder.paging);
                var employeId = data.Select(u => u.FK_ADFromUserID).ToList();
                var employeInfo = new List<EmployeeBasicInfo>();
                if (employeId != null && employeId.Count > 0)
                    employeInfo = _commonService.getBasicEmployeeByListUserId(employeId);

                return Ok(_service.addingEdit(data, employeInfo));
            }
            else if (type == MailBoxType.OUTBOX)
            {
                var sqlBuilder = _outBoxService.buidingSQLClause(type,  userInfo, searchCondition);

                List<ADOutboxItemResponse> data = (List<ADOutboxItemResponse>)_outBoxService.GetPagingData(sqlBuilder.sqlCondition,
                    sqlBuilder.orderByCondition, sqlBuilder.paging);
                var employeId = data.Select(u => u.FK_ADFromUserID).ToList();
                var employeInfo = _commonService.getBasicEmployeeByListUserId(employeId);

                return Ok(_outBoxService.addingEdit(data, employeInfo));
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("detail/{objId}/{objectType}")]
        public IActionResult GetDetailData(int objId, string objectType)
        {
            // Get common infomation
            // switch case tuwngf laoi
            // _ = new object();
            object items;
            switch (objectType)
            {
                case ApproveType.HR_OFF:
                    items = _employeeService.getMyData("", new HREmployeeOffWorkSearch()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.HR_OT:
                    items = _otService.getMyData("", new HREmployeeOvertimeSearch()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.HR_TRAVEL:
                    items = _travelService.getMyData("", new HRTravelCalendarSearch()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.PAYMENT_REQUEST:
                    items = _simpleService.getVoucherPaymentMyData("", new ApproveSearchBase()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.PURCHASE_ORDER:
                    items = _simpleService.getAPPOMyData("", new ApproveSearchBase()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.PURCHASE_REQUEST:
                    items = _simpleService.getAPPRMyData("", new ApproveSearchBase()
                    {
                        id = objId
                    });
                    return Ok(items);
                case ApproveType.SALE_ORDER:
                    items = _simpleService.getSOMyData("", new ApproveSearchBase()
                    {
                        id = objId
                    });
                    return Ok(items);
                default:
                    //do a different thing
                    break;
            }
            return Ok();
        }
    }
}
