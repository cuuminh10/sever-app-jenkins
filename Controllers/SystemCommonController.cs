using gmc_api.DTO.CommonData;
using gmc_api.Base.Helpers;
using gmc_api.DTO.HR;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using static gmc_api.Base.Helpers.Constants;
using gmc_api.Base.dto;
using gmc_api.Helpers;
using System.Linq;

namespace gmc_api.Controllers
{

    [ApiController()]
    [Route("common")]
    public class SystemCommonController : ControllerBase
    {
        private readonly ISystemCommonService _service;
        private readonly IADInboxItemService _inBoxService;
        private readonly IADOutboxItemService _outBoxService;
        private readonly IADDocHistoryService _historyService;
     //    private readonly FireBase _fireBase;

        public SystemCommonController(ISystemCommonService service, IADInboxItemService inBoxService, IADOutboxItemService outBoxService,
             IADDocHistoryService historyService)
        {
            _service = service;
            _inBoxService = inBoxService;
            _outBoxService = outBoxService;
           // _fireBase = fireBase;
            _historyService = historyService;
        }
        //
        // var dbTable = TableCommon.commonTable[table];
        [Authorize]
        [HttpGet("{tableType}")]
        public IActionResult GetAll(string tableType)
        {
           // _fireBase.AddFireBase("users", )
            var dbTable = TableCommon.commonTable[tableType];
            if (string.IsNullOrEmpty(dbTable))
            {
                return BadRequest(new { message = "Data input not correct!!!" });
            }
            var users = _service.GetDisplayData(dbTable);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("getComment/{type}/{id}")]
        public IActionResult GetComment(string type, int id)
        {
            if (!ApproveType.isIn(type))
            {
                return BadRequest(new { message = ApproveType.messageValidate() });
            }
            return Ok(_service.GetComment(type, id));
        }

        [Authorize]
        [HttpGet("getProcessApprove/{type}/{id}")]
        public IActionResult GetApproveProcess(string type, int id)
        {
            if (!ApproveType.isIn(type))
            {
                return BadRequest(new { message = ApproveType.messageValidate() });
            }
            return Ok(_service.GetApproveProcess(type, id));
        }

        [Authorize]
        [HttpGet("sendApprove/{type}/{id}")]
        public IActionResult SendApprove(string type, int id)
        {
            if (!ApproveType.isIn(type))
            {
                return BadRequest(new { message = ApproveType.messageValidate() });
            }
            var approveCheck = _service.CheckContainsApproveProcess(type);
            if (approveCheck.counts == 0)
            {
                // Auto approve - update table
                _service.UpdateStatusMainObject(type, id, ApproveStatus.APROVED);
                return Ok(new { status = ApproveStatus.APROVED });
            }
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var approveData = _service.GetDetailApproveProcess(type, id, userInfo.UserName);
            if (approveData == null)
            {
                return BadRequest(new { message = "Người dùng chưa được cấu hình phê duyệt" });
            }
            if (string.IsNullOrEmpty(approveData.ADMailCCUsers))
            {
                _service.UpdateStatusMainObject(type, id, ApproveStatus.APROVED, approveData.FK_ADApprovalProcID);
                return Ok(new { status = ApproveStatus.APROVED });
            }
            // Update staus inprocess - update table
            _service.UpdateStatusMainObject(type, id, ApproveStatus.INPROCESS, approveData.FK_ADApprovalProcID);
            // Insert inpbox - outbox
            EmployeeBasicInfo empInfo = _service.getBasicEmployeeByUserId(userInfo.UserID);
            approveData.FK_ADFromUserID = userInfo.UserID;
            approveData.FK_HRFromEmployeeID = empInfo == null ? 0 : empInfo.employeeId;

            _inBoxService.CreateObject(approveData, userInfo);
            var outBoxData = _service.CovertInBoxToOutBox(approveData);
            _outBoxService.CreateObject(outBoxData, userInfo);

            var history = _service.CovertInBoxToHistory(approveData, userInfo.UserName, _service.getPositionNo(empInfo.positionID), id);
            _historyService.CreateObject(history);

            // Send cho nguoi duyet
            var userSendNotifyId = _service.getUserIdByName(approveData.ADMailToUsers.Split(";").ToList<string>()
                .Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList());
            var _fireBase = new FireBase();
            userSendNotifyId.ForEach(u =>
               {
                   _fireBase.AddFireBase("user", u.counts, new DTO.FireBase.UserFireBase()
                   {
                       content = approveData.ADInboxItemSubject,
                       readFlag = false,
                       objectId = approveData.ADInboxItemObjectID,
                       sndUser = userInfo.UserName,
                       sndDate = System.DateTime.Now,
                       type = approveData.ADInboxItemDocType
                   });
               });
            
            return Ok(new { lc_status = ApproveStatus.INPROCESS });
        }

        [Authorize]
        [HttpPost("approve/{type}")]
        public IActionResult Approve(string type, ApproveDataRequest input)
        {
            if (!ApproveType.isIn(type))
            {
                return BadRequest(new { message = ApproveType.messageValidate() });
            }
            if (input.status != ApproveStatus.APROVED
                && input.status != ApproveStatus.REJECT)
            {
                return BadRequest(new { message = ApproveStatus.messageValidate() });
            }
            //  var approveCheck = _service.CheckContainsApproveProcess(type);
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var approveData = _service.GetNextDetailApproveProcess(type, input.inboxId, input.objectId, input.status, input.reasion, input.approveStepID, userInfo.UserName);
            if (approveData == null)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không đúng hoặc phiếu đã được phê duyệt" });
            }
            EmployeeBasicInfo empInfo = _service.getBasicEmployeeByUserId(userInfo.UserID);
            string statusReturn;
            if (approveData[0].ADInboxItemProtocol == approveData[1].ADInboxItemProtocol)
            {
                _service.UpdateStatusMainObject(type, input.objectId, input.status);
                statusReturn = input.status;
            }
            else
            {
                _service.UpdateStatusMainObject(type, input.objectId, ApproveStatus.APROVVING);
                statusReturn = ApproveStatus.APROVVING;
            }
            bool flagAdd = false;
            var _fireBase = new FireBase();
            for (int i = 0; i < approveData.Count; i++)
            {
                var currentData = approveData[i];
                currentData.FK_ADFromUserID = userInfo.UserID;
                currentData.FK_HRFromEmployeeID = empInfo == null ? 0 : empInfo.employeeId;

                if (currentData.type == MailBoxType.BOTH ||
                    currentData.type == MailBoxType.INBOX)
                {
                    _inBoxService.CreateObject(approveData[i], userInfo);
                }
                if (currentData.type == MailBoxType.BOTH ||
                    currentData.type == MailBoxType.OUTBOX)
                {
                    var outBoxData = _service.CovertInBoxToOutBox(currentData);
                    _outBoxService.CreateObject(outBoxData, userInfo);
                }
                if(currentData.ADInboxItemProtocol == MailBoxProtocolType.Message
                    && !flagAdd)
                {
                    flagAdd = true;
                    var history = _service.CovertInBoxToHistory(currentData, userInfo.UserName, _service.getPositionNo(empInfo.positionID),input.objectId );
                    _historyService.CreateObject(history);
                }

                // Send cho nguoi duyet
                var userSendNotifyId = _service.getUserIdByName(currentData.ADMailToUsers.Split(";").ToList<string>()
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList());
                userSendNotifyId.ForEach(u =>
                {
                    _fireBase.AddFireBase("user", u.counts, new DTO.FireBase.UserFireBase()
                    {
                        content = currentData.ADInboxItemSubject,
                        readFlag = false,
                        objectId = currentData.ADInboxItemObjectID,
                        sndUser = userInfo.UserName,
                        sndDate = System.DateTime.Now,
                        type = currentData.ADInboxItemDocType
                    });
                });
            }
            _service.UpdateStatusMailOuboxObject(MailBoxType.OUTBOX, _service.GetOutBoxFromInBoxId(input.inboxId), input.status);
            _service.UpdateStatusMailOuboxObject(MailBoxType.INBOX, input.inboxId, input.status);
            return Ok(new { lc_status = statusReturn });
        }

        [Authorize]
        [HttpPost("comment/{type}")]
        public IActionResult CommentApprove(string type, ApproveCommentRequest input)
        {
            if (!ApproveType.isIn(type))
            {
                return BadRequest(new { message = ApproveType.messageValidate() });
            }
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            EmployeeBasicInfo empInfo = _service.getBasicEmployeeByUserId(userInfo.UserID);
            var inbox = _service.GetInboxItemById(input.inboxId, input.objectId, type, input.comment, userInfo, empInfo);
            var commentFireBase = inbox.ADInboxItemAction;
            inbox.ADInboxItemAction = "Comment";
            _inBoxService.CreateObject(inbox, userInfo);
            var outBoxData = _service.CovertInBoxToOutBox(inbox);
            _outBoxService.CreateObject(outBoxData, userInfo);
            var userSendNotifyId = _service.getUserIdByName(inbox.ADMailToUsers.Split(";").ToList<string>()
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList());
            var _fireBase = new FireBase();
            userSendNotifyId.ForEach(u =>
            {
                _fireBase.AddFireBase("user", u.counts, new DTO.FireBase.UserFireBase()
                {
                    content = commentFireBase,
                    readFlag = false,
                    objectId = inbox.ADInboxItemObjectID,
                    sndUser = userInfo.UserName,
                    sndDate = System.DateTime.Now,
                    type = type
                });
            });
            return Ok(new { commnent = input.comment });
        }

        [Authorize]
        [HttpPost("cancelTicket/{type}/{objectId}")]
        public IActionResult CancelTicket(string type, int objectId)
        {
            _service.updateStatusApprove(type, objectId);
            return Ok(new { lc_status = "New" , displayReject = 0 });
        }

        [Authorize]
        [HttpPost("noti/read/{id}")]
        public IActionResult UpdatReadNotify(string id)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var _fireBase = new FireBase();
            _fireBase.UpdateReadFlagFireBase("user", userInfo.UserID, id);
            return Ok();
        }
    }
}
