using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.dto;
using gmc_api.Base.dto.Product;
using gmc_api.Base.InterFace;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.dto;
using gmc_api.DTO.HR;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Repositories
{
    public interface ISystemCommonReponsitory : IRepositoriesBase<DtoGMCBase, DtoGMCBase>
    {
        IEnumerable<CommonInfo> GetDisplayData(string table);
        EmployeeBasicInfo getBasicEmployeeByUserId(int userId);
        EmployeeBasicInfo getBasicEmployeeByUserName(string userName);
        OnePropIntReturn CheckContainsApproveProcess(string type);
        ADInboxItemInfo GetDetailApproveProcess(string type, int objectId, string userName);
        List<ADInboxItemInfo> GetNextDetailApproveProcess(string type, int inboxId, int objectId, string aproveAction, string reasion, int procStep, string userName);
        TwoPropReturn getBasicInfoObject(string type, int objectId);
        List<EmployeeBasicInfo> getBasicEmployeeByListEmpId(List<int> userIds);
        List<EmployeeBasicInfo> getBasicEmployeeByListUserId(List<int> userIds);
        int UpdateStatusMainObject(string table, int id, string column, string status, int idApprovePro = -1);
        TwoPropReturn getUserSubmit(int inboxId);
        int GetOutBoxFromInBoxId(int id);
        ADInboxItems GetInboxItemById(int inboxId);
        public List<GeneralNoItemsInfo> GetNoItemInfo(string moduleName);
        public OnePropIntReturn CheckContainColumn(string table, string column);
        OnePropStringReturn GetValueByColumn(string cl_f_table, string cl_f_columnName, int f_id);
        OnePropStringReturn getNoWithMaxMapping(string table, string currentNo);
        NumberingGenNo getNumberingInfo(int tranfId);
        public OnePropIntReturn CheckHaveNextObjectByRoutingIDAndPhase(int iRoutingID, int iPPPhaseCfgID);
        public OnePropStringReturn GetItemMSRevision(int iProductMSID, int iWOID);
        public OnePropIntReturn GetMSID(int iProductMSID, string msRevision);
        public ICProductBasicInfo GetBasicProductInfo(int productId);
        public decimal GetUOMFactor(int iICFromUOMID, int iICToUOMID);
        public ICProductUOMs GetProductUOMByProductUOM(int iProductID, int iUOMID);
        public List<PPMSLayoutItemsInfo> GetListLayoutItemRevisionByWO(int iProductMSID, int iProductLayoutID, int iWOID);
        JobTicketItemsKQSXInfo getItemsKQSXByLayout(int layoutId);
        public int GetDefaultTranCfgIDByOrgTranCfgID(int piUserID, string psModName, int ipOrgTranCfg = 0);
        OnePropStringReturn getPositionNo(int positionID);
        List<OnePropIntReturn> getUserIdByName(List<string> userName);
        List<CommentHistory> GetComment(string type, int id);
        List<ApproveHistory> GetApproveProcess(string type, int id);
        ADInboxItems GetLastInboxItemByObjectId(int objectId, string tableName);
        ADInboxItems GetLastApproveInboxItemByObjectId(int objectId, string tableName);
        int updateStatusApprove(string type, int objectId);
        public TwoPropReturn getUserSubmitById(int objectId, string objectType);
    }

    public class SystemCommonReponsitory : RepositoriesBaseImpl<DtoGMCBase, DtoGMCBase>, ISystemCommonReponsitory
    {
        private readonly GMCContext _context;
        private readonly IMapper _mapper;
        public SystemCommonReponsitory(GMCContext context, IMapper mapper) : base(context, "Empty", "EmptyID")
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>This method will check approve process have/havae't setting in systems</summary>
        /// <param name="type">Type of approve. Ref ApproveType class</param>
        /// <returns>If return 0 , it mean not exist approve process (auto approve)
        /// else mean contains approve process
        /// </returns>
        public OnePropIntReturn CheckContainsApproveProcess(string type)
        {
            var sqlBuilding = String.Format(@"select count(*) counts from ADApprovalProcs where AAStatus = 'Alive' AND ADApprovalProcActive = 1 AND ADDocumentType = '{0}'", type);
            return _context.OnePropIntReturn.FromSqlRaw(sqlBuilding).First<OnePropIntReturn>();
        }

        public EmployeeBasicInfo getBasicEmployeeByUserId(int userId)
        {
            var sqlBuilding = String.Format(@"select FK_HRPositionID positionID, FK_HRSectionID sectionID, HREmployeeID employeeId, HREmployeeNo employeeNo, HREmployeeName employeeName, FK_ADUserID userId, FK_HRDepartmentID departmentID from HREmployees where FK_ADUserID = {0}", userId);
            return _context.EmployeeBasicInfo.FromSqlRaw(sqlBuilding).FirstOrDefault<EmployeeBasicInfo>();
        }

        public EmployeeBasicInfo getBasicEmployeeByUserName(string userName)
        {
            var sqlBuilding = String.Format(@"select hr.FK_HRPositionID positionID, hr.FK_HRSectionID sectionID, hr.HREmployeeID employeeId, hr.HREmployeeNo employeeNo, hr.HREmployeeName employeeName, hr.FK_ADUserID userId, hr.FK_HRDepartmentID departmentID  from HREmployees hr
inner join ADUsers u ON hr.FK_ADUserID = u.ADUserID where u.ADUserName = '{0}'", userName);
            return _context.EmployeeBasicInfo.FromSqlRaw(sqlBuilding).FirstOrDefault<EmployeeBasicInfo>();

        }
        /// <summary>This method will get infomation approve process first level</summary>
        /// <param name="type">Type of approve. Ref ApproveType class</param>
        /// <param name="userName">User login name</param>
        /// <returns>If ApproveDataInfo 
        ///            + approveId = 0: Not exist approve process => không làm gì cả
        ///            + Else ccUser = '' : Not exist level approve => không làm gì cả
        ///            + else mean contains approve process : 
        ///                 - Inbox: them loại approve - to approveUser - cc ccUser
        ///                 - Outbox: thêm to approveUser - cc ccUser
        ///                 - Update status của đối tượng sang inprocessing
        /// </returns>
        public ADInboxItemInfo GetDetailApproveProcess(string type, int objectId, string userName)
        {
            var objectInfo = getBasicInfoObject(type, objectId);
            ADInboxItemInfo info = new();
            //Get all account level 1 in ApproveProcess
            var sqlBuilding = String.Format(@"SELECT 
	tmp.ADApprovalProcStepID approveStepId,
	tmp.FK_ADApprovalProcID approveId, 
	tmp.AssignUsers approveUser,
    tmp.AssignPositions ccUser,
    tmp.ADApprovalProcStepID nextApproveStepId,
    tmp.RowNumber rowNumber,
    tmp.ADApprovalProcStepLevel currentLevel,
	tmp.ADApprovalProcStepRejectToLevel preLevel,
	tmp.ADApprovalProcStepApproveToLevel nextLevel
FROM
  (SELECT aps.ADApprovalProcStepID,
          aps.FK_ADApprovalProcID,
          concat(aps.AssignUsers,'---', aps.AssignPositions) AssignUsers,
          concat(aps.CCUsers,'---', aps.CCPositions)  AssignPositions,
          aps.ADApprovalProcStepLevel,
	      aps.ADApprovalProcStepRejectToLevel,
	      aps.ADApprovalProcStepApproveToLevel,
          ROW_NUMBER() OVER(PARTITION BY aps.FK_ADApprovalProcID
                            ORDER BY aps.ADApprovalProcStepLevel ASC) AS RowNumber
FROM ADApprovalProcs ap
   INNER JOIN ADApprovalProcSteps aps ON ap.ADApprovalProcID = aps.FK_ADApprovalProcID
   AND aps.AAStatus = 'Alive'
   WHERE ap.AAStatus = 'Alive' AND ap.ADApprovalProcActive = 1
     AND ap.ADDocumentType = '{0}' ) tmp
order by approveId , rowNumber", type);

            var dataAproveAll = _context.ApproveDataInfo.FromSqlRaw(sqlBuilding).ToList<ApproveDataInfo>();
            var dataApproveSelected = new ApproveDataInfo();
            var tmpCCUser = new List<string>();
            bool hitApproveData = false;
            // var dataAprove = dataAproveAll.Where(s => s.rowNumber == 1).ToList();
            for (int i = 0; i < dataAproveAll.Count; i++)
            {
                var el = dataAproveAll[i];
                var appUserPart = el.approveUser.Split("---");
                if (!string.IsNullOrEmpty(appUserPart[0]) && appUserPart[0].Split(";").Contains(userName))
                {
                    hitApproveData = true;
                    dataApproveSelected = el;
                }
                if (!string.IsNullOrEmpty(appUserPart[1]) && !hitApproveData)
                {
                    tmpCCUser = appUserPart[1].Split(";").ToList<string>();
                    tmpCCUser = tmpCCUser.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    if (tmpCCUser.Count > 0)
                    {
                        List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpCCUser);
                        for (int j = 0; j < rsGetUserByPostion.Count; j++)
                        {
                            if (rsGetUserByPostion[j].dataReturn.Equals(userName))
                            {
                                hitApproveData = true;
                                dataApproveSelected = el;
                            }
                        }
                    }
                }
                if (hitApproveData)
                {
                    dataApproveSelected.ccUser = "";
                    dataApproveSelected.approveUser = "";
                    dataApproveSelected.nextApproveStepId = 0;
                    break;
                }
            };
            if (hitApproveData)
            {
                var dataAproveNextLevel = dataAproveAll.Where(s => s.rowNumber == (dataApproveSelected.rowNumber + 1) && s.approveId == dataApproveSelected.approveId).FirstOrDefault<ApproveDataInfo>();
                if (dataAproveNextLevel == null)
                {
                    info.ADMailToUsers = "";
                    info.ADInboxItemProtocol = MailBoxProtocolType.APPROVE;
                    info.ADInboxItemObjectID = objectId;
                    return info;
                }
                else
                {
                    dataApproveSelected.nextApproveStepId = dataAproveNextLevel.approveStepId;
                    var appUserPart = dataAproveNextLevel.approveUser.Split("---");
                    var tmpUserPartPosition = appUserPart[1].Split(";").ToList<string>();
                    tmpUserPartPosition = tmpUserPartPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    if (tmpUserPartPosition.Count > 0)
                    {
                        List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartPosition);
                        var tmpPositionUser = "";
                        for (int j = 0; j < rsGetUserByPostion.Count; j++)
                        {
                            tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                        }
                        dataApproveSelected.approveUser = appUserPart[0] + tmpPositionUser;
                    }
                    else
                    {
                        dataApproveSelected.approveUser = appUserPart[0];
                    }

                    var appUserCCPart = dataAproveNextLevel.ccUser.Split("---");
                    var tmpUserPartCCPosition = appUserCCPart[1].Split(";").ToList<string>();
                    tmpUserPartCCPosition = tmpUserPartCCPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    if (tmpUserPartCCPosition.Count > 0)
                    {
                        List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartCCPosition);
                        var tmpPositionUser = "";
                        for (int j = 0; j < rsGetUserByPostion.Count; j++)
                        {
                            tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                        }
                        dataApproveSelected.ccUser = appUserCCPart[0] + tmpPositionUser;
                    }
                    else
                    {
                        dataApproveSelected.ccUser = appUserCCPart[0];
                    }
                }
                info.FK_ADApprovalProcID = dataApproveSelected.approveId;
                info.FK_ADApprovalProcStepID = dataApproveSelected.nextApproveStepId;
                info.ADMailToUsers = dataApproveSelected.approveUser;
                info.ADMailCCUsers = dataApproveSelected.ccUser;
                info.type = MailBoxType.BOTH;
                info.ADInboxItemProtocol = MailBoxProtocolType.APPROVE;
                info.ADInboxItemObjectID = objectId;
                info.ADInboxItemTableName = ApproveType.ApproveTable[type];
                info.ADInboxItemDocApprovalStatusCombo = "New";
                info.ADInboxItemAction = "";
                info.ADInboxItemTaskStatusCombo = "Submit";
                info.ADInboxItemPriorityCombo = "Medium";
                info.ADInboxItemDocType = type;
                info.ADInboxItemDocNo = objectInfo.dataFirst;
                info.ADInboxItemSubject = String.Format(@"{0} submit {1} mã số {2}", userName, ApproveType.ApproveTitle[type], objectInfo.dataFirst);
                return info;
            }
            return null;

        }

        private List<OnePropStringReturn> getUserByPosition(List<string> tmpCCUser)
        {
            var sqlGetUserByPostion = string.Format(@"select distinct u.ADUserName dataReturn from HRPositions hrp
inner join HREmployees hre on hrp.HRPositionID = hre.FK_HRPositionID AND hre.AAStatus = 'Alive'
inner join ADUsers u on u.ADUserID = hre.FK_ADUserID AND u.AAStatus = 'Alive'
WHERE hrp.HRPositionNo in ({0}) and hrp.AAStatus = 'Alive'", string.Join(", ", tmpCCUser.Select(s => string.Format("'{0}'", s))));
            var rsGetUserByPostion = _context.OnePropStringReturn.FromSqlRaw(sqlGetUserByPostion).ToList<OnePropStringReturn>();
            return rsGetUserByPostion;
        }

        public IEnumerable<CommonInfo> GetDisplayData(string table)
        {
            var columnAppends = table.Split("_");
            var columnAppend = "No";
            var foregnId = "";
            if (columnAppends.Length > 1)
                columnAppend = columnAppends[1];
            if (columnAppends.Length == 3)
            {
                foregnId = columnAppends[2];
            }
            var tableNameCommon = columnAppends[0][0..^1];
            var sqlBuilding = String.Format(@"select {0} objId, {1} objNo, {2} foreignId
from {3} where AAStatus = 'Alive' ", tableNameCommon + "ID", tableNameCommon + columnAppend, foregnId.Length == 0 ? -1 : string.Format(@"FK_{0}", foregnId), columnAppends[0]);
            return _context.CommonInfo.FromSqlRaw(sqlBuilding).ToList<CommonInfo>();
        }

        /// <summary>This method will get infomation approve process current and next level</summary>
        /// <param name="type">Type of approve. Ref ApproveType class</param>
        /// <param name="userName">User login name</param>
        /// <returns>If ApproveDataInfo 
        ///            + approveId = 0: Not exist data -> không làm gì cả
        ///            + Else curent = next step : 
        ///                 - Inbox: 
        ///                 - Outbox: thêm to approveUser - cc ccUser
        ///                 - Update status của đối tượng sang approve
        ///                 - Update ADInboxItemAction của record gởi sang trong inbox
        ///            + else :
        ///                 - Inbox: them loại approve - to approveUser - cc ccUser
        ///                 - Outbox: thêm to approveUser - cc ccUser
        ///                 - Update status của đối tượng sang approving
        /// </returns>
        public List<ADInboxItemInfo> GetNextDetailApproveProcess(string type, int inboxId, int objectId, string aproveAction, string reasion, int procStep, string userName)
        {
            var objectInfo = getBasicInfoObject(type, objectId);
            var userSubmit = getUserSubmit(inboxId);
            var messageInfo = new List<ADInboxItemInfo>();
            // Get approve process infomation
            var sqlApproveBuilding = String.Format(@"SELECT 
	aps.ADApprovalProcStepID approveStepId,
	aps.FK_ADApprovalProcID approveId, 
    concat(aps.AssignUsers,'---', aps.AssignPositions) approveUser,
     concat(aps.CCUsers,'---', aps.CCPositions)  ccUser,
    aps.ADApprovalProcStepID nextApproveStepId,
    CAST(0 AS bigint ) rowNumber,
    aps.ADApprovalProcStepLevel currentLevel,
	aps.ADApprovalProcStepRejectToLevel preLevel,
	aps.ADApprovalProcStepApproveToLevel nextLevel
FROM ADApprovalProcs ap
   INNER JOIN ADApprovalProcSteps aps ON ap.ADApprovalProcID = aps.FK_ADApprovalProcID AND aps.AAStatus = 'Alive'
   WHERE ap.AAStatus = 'Alive' and aps.ADApprovalProcStepID = {0}", procStep);
            var dataCurrentStep = _context.ApproveDataInfo.FromSqlRaw(sqlApproveBuilding).FirstOrDefault();
            if (dataCurrentStep == null || dataCurrentStep.approveId == 0)
            {
                return null;
            }
            // Get current step postion 
            var appUserPart = dataCurrentStep.ccUser.Split("---");
            var tmpUserPartPosition = appUserPart[1].Split(";").ToList<string>();
            tmpUserPartPosition = tmpUserPartPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            var currentStepCCUser = appUserPart[0];
            if (tmpUserPartPosition.Count > 0)
            {
                List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartPosition);
                for (int j = 0; j < rsGetUserByPostion.Count; j++)
                {
                    currentStepCCUser += rsGetUserByPostion[j].dataReturn + ";";
                }
            }
            //Get all account level 1 in ApproveProcess
            var sqlNextBuilding = String.Format(@"SELECT 
	tmp.ADApprovalProcStepID approveStepId,
	tmp.FK_ADApprovalProcID approveId, 
	tmp.AssignUsers approveUser,
    tmp.AssignPositions ccUser,
    tmp.ADApprovalProcStepID nextApproveStepId,
    tmp.RowNumber rowNumber,
    tmp.ADApprovalProcStepLevel currentLevel,
	tmp.ADApprovalProcStepRejectToLevel preLevel,
	tmp.ADApprovalProcStepApproveToLevel nextLevel
FROM
  (SELECT aps.ADApprovalProcStepID,
          aps.FK_ADApprovalProcID,
          concat(aps.AssignUsers,'---', aps.AssignPositions) AssignUsers,
          concat(aps.CCUsers,'---', aps.CCPositions)  AssignPositions,
          aps.ADApprovalProcStepLevel,
	      aps.ADApprovalProcStepRejectToLevel,
	      aps.ADApprovalProcStepApproveToLevel,
          ROW_NUMBER() OVER(PARTITION BY aps.FK_ADApprovalProcID
                            ORDER BY aps.ADApprovalProcStepLevel ASC) AS RowNumber
FROM ADApprovalProcs ap
   INNER JOIN ADApprovalProcSteps aps ON ap.ADApprovalProcID = aps.FK_ADApprovalProcID
   AND aps.AAStatus = 'Alive'
   WHERE ap.AAStatus = 'Alive'
     AND ap.ADDocumentType = '{0}' and (aps.ADApprovalProcStepApproveToLevel = {1} or aps.ADApprovalProcStepRejectToLevel = {1}) and aps.FK_ADApprovalProcID = {2} ) tmp
WHERE tmp.RowNumber in(1,2)", type, dataCurrentStep.currentLevel, dataCurrentStep.approveId);

            var dataAproveAll = _context.ApproveDataInfo.FromSqlRaw(sqlNextBuilding).ToList<ApproveDataInfo>();

            //       var preApprove = "";
            //    var preCC = "";
            var returnData = new ApproveDataInfo();
            var returnPreData = new ApproveDataInfo();
            var dataPreStep = dataAproveAll.Where(s => s.rowNumber == 1).FirstOrDefault<ApproveDataInfo>();
            if (dataPreStep == null)
            {
                return null;
            }
            else
            {
                // lấy to
                var appUserPrePart = dataPreStep.approveUser.Split("---");
                //  preApprove = appUserPrePart[0];
                var tmpUserPrePartPosition = appUserPrePart[1].Split(";").ToList<string>();
                tmpUserPrePartPosition = tmpUserPrePartPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                if (tmpUserPrePartPosition.Count > 0)
                {
                    List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPrePartPosition);
                    var tmpPositionUser = "";
                    for (int j = 0; j < rsGetUserByPostion.Count; j++)
                    {
                        tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                    }
                    returnPreData.approveUser = appUserPrePart[0] + tmpPositionUser;
                }
                else
                {
                    returnPreData.approveUser = appUserPrePart[0];
                }
                // lấy CC
                var appUserPrePartCC = dataPreStep.ccUser.Split("---");
                var tmpUserPartCCPosition = appUserPrePartCC[1].Split(";").ToList<string>();
                tmpUserPartCCPosition = tmpUserPartCCPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                if (tmpUserPartCCPosition.Count > 0)
                {
                    List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartCCPosition);
                    var tmpPositionUser = "";
                    for (int j = 0; j < rsGetUserByPostion.Count; j++)
                    {
                        tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                    }
                    returnPreData.ccUser = appUserPrePartCC[0] + tmpPositionUser;

                }
                else
                {
                    returnPreData.ccUser = appUserPrePartCC[0];
                }
                if (dataCurrentStep.currentLevel == dataCurrentStep.nextLevel
                    || aproveAction == ApproveStatus.REJECT)
                {
                    // Send messgae info approved/reject cho người ở level trước
                    returnPreData.nextApproveStepId = 0;
                    ADInboxItemInfo infoApprove = new()
                    {
                        // infoApprove = _mapper.Map<MessageDataInfo>(returnData);
                        type = MailBoxType.INBOX,
                        ADInboxItemProtocol = MailBoxProtocolType.Message,
                        FK_ADApprovalProcID = dataCurrentStep.approveId,
                        FK_ADApprovalProcStepID = dataCurrentStep.nextApproveStepId,
                        ADMailToUsers = userSubmit.dataFirst + ";",
                        ADMailCCUsers = currentStepCCUser,//returnPreData.ccUser + ";" +
                        ADInboxItemMessage = reasion,
                        ADInboxItemSubject = String.Format(@"{0} {1} {2} mã số {3}", userName, aproveAction, ApproveType.ApproveTitle[type], objectInfo.dataFirst),
                        ADInboxItemTableName = ApproveType.ApproveTable[type],
                        ADInboxItemDocApprovalStatusCombo = "New",
                        ADInboxItemAction = aproveAction,
                        ADInboxItemTaskStatusCombo = "Submit",
                        ADInboxItemPriorityCombo = "Medium",
                        ADInboxItemDocType = type,
                        ADInboxItemObjectID = objectId,
                        ADInboxItemDocNo = objectInfo.dataFirst
                    };
                    // Send messgae info thông báo hoàn thành cho nguoi submit
                    ADInboxItemInfo infoCompleted = new()
                    {
                        //infoCompleted = _mapper.Map<MessageDataInfo>(returnData);
                        type = MailBoxType.BOTH,
                        ADInboxItemProtocol = MailBoxProtocolType.Message,
                        FK_ADApprovalProcID = dataCurrentStep.approveId,
                        FK_ADApprovalProcStepID = dataCurrentStep.nextApproveStepId,
                        ADMailToUsers = objectInfo.dataSecond + ";",
                        ADMailCCUsers = "",
                        ADInboxItemMessage = reasion,
                        ADInboxItemObjectID = objectId,
                        ADInboxItemSubject = String.Format(@"{0} mã số {1} : {2} ", ApproveType.ApproveTitle[type], objectInfo.dataFirst, aproveAction.Equals(ApproveStatus.APROVED) ?
                            string.Format("đã được duyệt bởi {0}", userName) : string.Format("đã từ chối bởi {0}", userName)),
                        ADInboxItemTableName = ApproveType.ApproveTable[type],
                        ADInboxItemDocApprovalStatusCombo = aproveAction,
                        ADInboxItemAction = aproveAction,
                        ADInboxItemTaskStatusCombo = "New",
                        ADInboxItemPriorityCombo = "Medium",
                        ADInboxItemDocType = type,
                        ADInboxItemDocNo = objectInfo.dataFirst
                    };

                    messageInfo.Add(infoCompleted);
                    messageInfo.Add(infoApprove);
                    return messageInfo;
                }
            }
            var dataAproveNext = dataAproveAll.Where(s => s.rowNumber == 2).FirstOrDefault<ApproveDataInfo>();
            if (dataAproveNext == null ||
                aproveAction == ApproveStatus.REJECT)
            {
                return null;
            }
            else
            {
                returnData = dataAproveNext;
                returnData.nextApproveStepId = dataAproveNext.approveStepId;
                var appUserPartL2 = dataAproveNext.approveUser.Split("---");
                var tmpUserPartL2Position = appUserPartL2[1].Split(";").ToList<string>();
                tmpUserPartL2Position = tmpUserPartL2Position.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                if (tmpUserPartL2Position.Count > 0)
                {
                    List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartL2Position);
                    var tmpPositionUser = "";
                    for (int j = 0; j < rsGetUserByPostion.Count; j++)
                    {
                        tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                    }
                    returnData.approveUser = appUserPartL2[0] + tmpPositionUser;
                }
                else
                {
                    returnData.approveUser = appUserPartL2[0];
                }

                var appUserCCPart = dataAproveNext.ccUser.Split("---");
                var tmpUserPartCCPosition = appUserCCPart[1].Split(";").ToList<string>();
                tmpUserPartCCPosition = tmpUserPartCCPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                if (tmpUserPartCCPosition.Count > 0)
                {
                    List<OnePropStringReturn> rsGetUserByPostion = getUserByPosition(tmpUserPartCCPosition);
                    var tmpPositionUser = "";
                    for (int j = 0; j < rsGetUserByPostion.Count; j++)
                    {
                        tmpPositionUser += rsGetUserByPostion[j].dataReturn + ";";
                    }
                    returnData.ccUser = appUserCCPart[0] + tmpPositionUser;
                }
                else
                {
                    returnData.ccUser = appUserCCPart[0];
                }
                // Send message aprrove for next level
                ADInboxItemInfo infoSumitApprove = new()
                {
                    // infoSumitApprove = _mapper.Map<MessageDataInfo>(returnData);
                    type = MailBoxType.BOTH,
                    ADInboxItemProtocol = MailBoxProtocolType.APPROVE,
                    FK_ADApprovalProcID = returnData.approveId,
                    FK_ADApprovalProcStepID = returnData.nextApproveStepId,
                    ADMailToUsers = returnData.approveUser,
                    ADMailCCUsers = returnData.ccUser,
                    ADInboxItemMessage = reasion,
                    ADInboxItemObjectID = objectId,
                    ADInboxItemSubject = String.Format(@"{0} submit {1} mã số {2}", userName, ApproveType.ApproveTitle[type], objectInfo.dataFirst),
                    ADInboxItemTableName = ApproveType.ApproveTable[type],
                    ADInboxItemDocApprovalStatusCombo = "",
                    ADInboxItemAction = "",
                    ADInboxItemTaskStatusCombo = "Submit",
                    ADInboxItemPriorityCombo = "Medium",
                    ADInboxItemDocType = type,
                    ADInboxItemDocNo = objectInfo.dataFirst
                };

                // Send message inform for pre level
                ADInboxItemInfo infoSumitPreApprove = new()
                {
                    //infoSumitApprove = _mapper.Map<MessageDataInfo>(returnData);
                    type = MailBoxType.BOTH,
                    ADInboxItemProtocol = MailBoxProtocolType.Message,
                    FK_ADApprovalProcID = returnData.approveId,
                    FK_ADApprovalProcStepID = returnData.nextApproveStepId,
                    ADMailToUsers = userSubmit.dataFirst + ";",
                    ADMailCCUsers = currentStepCCUser,
                    ADInboxItemMessage = reasion,
                    ADInboxItemSubject = String.Format(@"{0} {1} {2} mã số {3}", userName, aproveAction, ApproveType.ApproveTitle[type], objectInfo.dataFirst),
                    ADInboxItemTableName = ApproveType.ApproveTable[type],
                    ADInboxItemDocApprovalStatusCombo = "",
                    ADInboxItemAction = aproveAction,
                    ADInboxItemTaskStatusCombo = "Submit",
                    ADInboxItemPriorityCombo = "Medium",
                    ADInboxItemObjectID = objectId,
                    ADInboxItemDocType = type,
                    ADInboxItemDocNo = objectInfo.dataFirst
                };

                messageInfo.Add(infoSumitApprove);
                messageInfo.Add(infoSumitPreApprove);
                return messageInfo;
            }
        }


        public TwoPropReturn getBasicInfoObject(string type, int objectId)
        {
            string tableName = ApproveType.ApproveTable[type];
            string tableNameProp = tableName.Substring(0, tableName.Length - 1);
            var sqlBuilding = string.Format(@"select {0} dataFirst, {1} dataSecond from {2} where AAStatus = 'Alive' AND {3} = {4}", tableNameProp + "No",
               "AACreatedUser", tableName, tableNameProp + "ID", objectId);
            var data = _context.TwoPropReturn.FromSqlRaw(sqlBuilding).First<TwoPropReturn>();
            if (CheckContainColumn(tableName, "FK_HREmployeeID") != null
                && string.IsNullOrEmpty(data.dataSecond) && data != null)
            {
                sqlBuilding = string.Format(@"select u.ADUserName dataFirst, u.AAStatus dataSecond from ADUsers u 
inner join HREmployees hr on hr.FK_ADUserID = u.ADUserID  
inner join {0} mains on mains.FK_HREmployeeID = hr.HREmployeeID where u.AAStatus = 'Alive' AND {1} = {2}", tableName, tableNameProp + "ID", objectId);
                var dataTmp = _context.TwoPropReturn.FromSqlRaw(sqlBuilding).FirstOrDefault<TwoPropReturn>();
                if (dataTmp != null)
                    data.dataSecond = dataTmp.dataFirst;
            }

            return data ?? new TwoPropReturn();
        }

        public List<EmployeeBasicInfo> getBasicEmployeeByListEmpId(List<int> userIds)
        {
            var sqlBuilding = String.Format(@"select FK_HRPositionID positionID, FK_HRSectionID sectionID, HREmployeeID employeeId, HREmployeeNo employeeNo, HREmployeeName employeeName,0 userId, FK_HRDepartmentID departmentID
from HREmployees where HREmployeeID IN ({0})", string.Join(", ", userIds.Select(s => string.Format("{0}", s))));
            return _context.EmployeeBasicInfo.FromSqlRaw(sqlBuilding).ToList<EmployeeBasicInfo>();
        }

        public List<EmployeeBasicInfo> getBasicEmployeeByListUserId(List<int> userIds)
        {
            var sqlBuilding = String.Format(@"select isnull(FK_HRPositionID,0) positionID, isnull(FK_HRSectionID,0) sectionID, isnull(HREmployeeID,0) employeeId, isnull(HREmployeeNo,'') employeeNo, 
case when HREmployeeName is null then ADUserName else HREmployeeName end employeeName, ad.ADUserID userId, isnull(FK_HRDepartmentID,0) departmentID
from ADUsers  ad left join HREmployees on FK_ADUserID = ADUserID
where ad.ADUserID IN ({0})", string.Join(", ", userIds.Select(s => string.Format("{0}", s))));
            return _context.EmployeeBasicInfo.FromSqlRaw(sqlBuilding).ToList<EmployeeBasicInfo>();
        }

        public int UpdateStatusMainObject(string table, int id, string column, string status, int idApprovePro = -1)
        {
            string tableNameProp = table.Substring(0, table.Length - 1);
            var appendApproveProc = idApprovePro == -1 ? "" : string.Format(@" , FK_ADApprovalProcID = {0} ", idApprovePro);
            var sqlBuilding = String.Format(@"UPDATE {0} SET {1} = '{2}' {3} WHERE {4} = {5} AND AAStatus = 'Alive'", table, column, status, appendApproveProc, tableNameProp + "ID", id);
            return _context.Database.ExecuteSqlRaw(sqlBuilding);
        }

        public TwoPropReturn getUserSubmit(int inboxId)
        {
            var sqlBuilding = String.Format(@"select u.ADUserName dataFirst,  'empty' dataSecond from ADInboxItems ib 
inner join ADUsers u on u.ADUserID = ib.FK_ADFromUserID where ib.AAStatus = 'Alive'  and ib.ADInboxItemID = {0}", inboxId);
            return _context.TwoPropReturn.FromSqlRaw(sqlBuilding).FirstOrDefault<TwoPropReturn>();
        }

        public TwoPropReturn getUserSubmitById(int objectId, string objectType)
        {
            var tableName = ApproveType.ApproveTable[objectType];
            var prefixColumns = tableName[0..^1];
            var sqlBuilding = String.Format(@"select AACreatedUser dataFirst,  'empty' dataSecond from {0}
where {1} = {2}", tableName, prefixColumns+"ID", objectId);
            return _context.TwoPropReturn.FromSqlRaw(sqlBuilding).FirstOrDefault<TwoPropReturn>();
        }

        public int GetOutBoxFromInBoxId(int id)
        {
            var sqlBuilding = String.Format(@"select outbox.ADOutboxItemID counts from ADOutboxItems outbox LEFT JOIN 
(select ADInboxItemID, FK_ADApprovalProcStepID, ADInboxItemObjectID ,FK_ADApprovalProcID,ADMailToUsers,FK_ADFromUserID,ADInboxItemProtocol from ADInboxiTEMS where ADInboxItemID = {0}
) inbox ON outbox.ADOutboxItemObjectID = inbox.ADInboxItemObjectID AND  outbox.FK_ADApprovalProcID = inbox.FK_ADApprovalProcID AND  outbox.ADMailToUsers = inbox.ADMailToUsers AND outbox.FK_ADFromUserID = inbox.FK_ADFromUserID AND  outbox.ADOutboxItemProtocol = inbox.ADInboxItemProtocol
WHERE  inbox.ADInboxItemID IS NOT NULL", id);
            return _context.OnePropIntReturn.FromSqlRaw(sqlBuilding).FirstOrDefault<OnePropIntReturn>().counts;
        }

        public ADInboxItems GetInboxItemById(int inboxId)
        {
            var sql = string.Format("SELECT * FROM dbo.ADInboxItems Where AAStatus = 'Alive' AND ADInboxItemID = {0}", inboxId);
            return _context.ADInboxItems.FromSqlRaw(sql).FirstOrDefault<ADInboxItems>();
        }

        public OnePropIntReturn CheckContainColumn(string table, string column)
        {
            var sql = string.Format("SELECT 1 counts FROM sys.columns WHERE Name = N'{0}' AND Object_ID = Object_ID(N'{1}')", column, table);
            return _context.OnePropIntReturn.FromSqlRaw(sql).FirstOrDefault<OnePropIntReturn>();
        }
        public List<GeneralNoItemsInfo> GetNoItemInfo(string moduleName)
        {
            var sql = string.Format(@"select gni.GEGenerateNoItemTypeCombo types,
gni.GEGenerateNoItemColumnName cl_n_columnName,
gni.GEGenerateNoItemForeignTableName cl_f_table,
gni.GEGenerateNoItemForeignColumnName cl_f_columnName,
gni.GEGenerateNoItemFormatCombo st_date_format,
gni.GEGenerateNoItemSeperatorCombo st_seper,
gni.GEGenerateNoItemValue st_type, --using for const
gni.GEGenerateNoItemOrderBy orders,
gni.GEGenerateNoItemLength st_auto_length from GEGenerateNos gn
inner join STModules st on st.STModuleID = gn.FK_STModuleID and st.STModuleName = '{0}'
inner join GEGenerateNoItems gni on gni.FK_GEGenerateNoID = gn.GEGenerateNoID
order by GEGenerateNoItemOrderBy", moduleName);
            return _context.GeneralNoItemsInfo.FromSqlRaw(sql).ToList<GeneralNoItemsInfo>();
        }

        public OnePropStringReturn GetValueByColumn(string f_table, string f_columnName, int f_id)
        {
            var tableColumns = f_table.Substring(0, f_table.Length - 1);
            var sql = string.Format("SELECT {0} dataReturn FROM {1} WHERE {2} = {3} ", f_columnName, f_table, tableColumns + "ID", f_id);
            return _context.OnePropStringReturn.FromSqlRaw(sql).FirstOrDefault<OnePropStringReturn>();
        }

        public OnePropStringReturn getNoWithMaxMapping(string table, string currentNo)
        {
            var tableColumns = table.Substring(0, table.Length - 1);
            var sql = string.Format("SELECT top 1 {0} dataReturn FROM {1} WHERE {2} like '{3}%' order by {4} DESC", tableColumns + "No", table, tableColumns + "No", currentNo,
                tableColumns + "No");
            return _context.OnePropStringReturn.FromSqlRaw(sql).FirstOrDefault<OnePropStringReturn>();
        }

        public NumberingGenNo getNumberingInfo(int tranfId)
        {
            var sql = string.Format(@"select  nu.GENumberingPrefix first_preFix,
case when GENumberingByDate = 1 then nu.GENumberingDateFormat else '' end snd_dateFormat,
nu.GENumberingSeparate third_separate,
case when GENumberingIsAutoChecked = 1 then nu.GENumberingLength else 0 end four_autoLength
from GLTranCfgs glTrans inner join GENumberings nu on glTrans.FK_GENumberingID = nu.GENumberingID
 where glTrans.GLTranCfgID = {0}", tranfId);
            return _context.NumberingGenNo.FromSqlRaw(sql).FirstOrDefault<NumberingGenNo>();
        }

        public OnePropIntReturn CheckHaveNextObjectByRoutingIDAndPhase(int iRoutingID, int iPPPhaseCfgID)
        {
            var sql = string.Format(@"SELECT count(1) counts
FROM dbo.PPRoutingOperations
INNER JOIN dbo.PPRoutings ON PPRoutings.PPRoutingID = PPRoutingOperations.FK_PPRoutingID
	AND PPRoutingOperations.AAStatus = 'Alive'
INNER JOIN dbo.PPPhaseCfgs ON PPPhaseCfgs.PPPhaseCfgID = PPRoutingOperations.FK_PPPhaseCfgID
	AND PPPhaseCfgs.AAStatus = 'Alive'
	AND PPPhaseCfgs.PPPhaseCfgNotProducedCheck = 0
WHERE PPRoutingOperations.AAStatus = 'Alive'
AND PPRoutingOperations.FK_PPRoutingID = {0}
AND PPRoutingOperations.PPRoutingOperationSortOrder > ( SELECT PPRoutingOperations.PPRoutingOperationSortOrder
		FROM dbo.PPRoutingOperations
		INNER JOIN dbo.PPRoutings ON PPRoutings.PPRoutingID = PPRoutingOperations.FK_PPRoutingID
										AND PPRoutingOperations.AAStatus = 'Alive'																																				
		INNER JOIN dbo.PPPhaseCfgs ON PPPhaseCfgs.PPPhaseCfgID = PPRoutingOperations.FK_PPPhaseCfgID
										AND PPPhaseCfgs.AAStatus = 'Alive'
										AND PPPhaseCfgs.PPPhaseCfgNotProducedCheck = 0
		WHERE PPRoutingOperations.AAStatus = 'Alive'
				AND PPRoutingOperations.FK_PPRoutingID = {0}
				AND (PPRoutingOperations.FK_PPPhaseCfgID = {1}))", iRoutingID, iPPPhaseCfgID);
            return _context.OnePropIntReturn.FromSqlRaw(sql).FirstOrDefault<OnePropIntReturn>();
        }

        public List<PPMSLayoutItemsInfo> GetListLayoutItemRevisionByWO(int iProductMSID, int iProductLayoutID, int iWOID)
        {
            var msRevision = GetItemMSRevision(iProductMSID, iWOID);
            var msId = GetMSID(iProductMSID, msRevision == null ? "" : msRevision.dataReturn);
            var sql = string.Format(@"SELECT  loi.*
FROM    dbo.PPMSs ms
INNER JOIN dbo.PPMSLayouts lo ON lo.FK_PPMSID = ms.PPMSID
	AND ms.AAStatus = 'Alive'
	AND lo.AAStatus = 'Alive'
INNER JOIN dbo.PPMSLayoutItems loi ON loi.FK_PPMSLayoutID = lo.PPMSLayoutID
	AND loi.AAStatus = 'Alive'
	AND loi.PPMSLayoutItemQty > 0
WHERE ms.PPMSID = {0} AND lo.FK_ICProductID = {1}", msId == null ? 0 : msId.counts, iProductLayoutID);
            return _context.PPMSLayoutItemsInfo.FromSqlRaw(sql).ToList<PPMSLayoutItemsInfo>();
        }
        public OnePropStringReturn GetItemMSRevision(int iProductMSID, int iWOID)
        {

            var sql = string.Format(@"SELECT TOP 1 isnull(wop.PPWOPItemMSRevision,'') dataReturn
FROM dbo.PPWOs wo
INNER JOIN dbo.PPWOPItems wop ON wop.FK_PPWOID = wo.PPWOID
		AND wop.AAStatus = wo.AAStatus
		AND wop.AAStatus = 'Alive'
WHERE wo.PPWOID = {0} AND wop.FK_ICProductID = {1}", iWOID, iProductMSID);
            return _context.OnePropStringReturn.FromSqlRaw(sql).FirstOrDefault<OnePropStringReturn>();
        }
        public OnePropIntReturn GetMSID(int iProductMSID, string msRevision)
        {

            var sql = string.Format(@"SELECT TOP 1 ms.PPMSID counts
FROM dbo.PPMSs ms
INNER JOIN dbo.PPMSItems msi ON msi.FK_PPMSID = ms.PPMSID
	AND msi.AAStatus = ms.AAStatus
	AND ms.AAStatus = 'Alive'
	AND msi.FK_PPMSItemID = 0
WHERE msi.FK_ICProductID = {0} AND ms.PPMSRevision = '{1}'
ORDER BY ms.PPMSActiveCheck DESC,ms.PPMSDate DESC", iProductMSID, msRevision);
            return _context.OnePropIntReturn.FromSqlRaw(sql).FirstOrDefault<OnePropIntReturn>();
        }

        public ICProductBasicInfo GetBasicProductInfo(int productId)
        {
            var sql = string.Format(@"SELECT ICProductWeight, ICProductVolume, ICProductGrossWeight, ICProductNetWeight, ICProductID, FK_ICStkUOMID FROM dbo.ICProducts WHERE ICProductID = {0} ", productId);
            return _context.ICProductBasicInfo.FromSqlRaw(sql).FirstOrDefault<ICProductBasicInfo>();
        }

        public decimal GetUOMFactor(int iICFromUOMID, int iICToUOMID)
        {
            decimal dbUOMFactor = 1;
            string sql = String.Format("Select * From [ICUOMFactors] Where [FK_ICFromUOMID]={0} And [FK_ICToUOMID]={1} AND AAStatus='Alive'", iICFromUOMID, iICToUOMID);
            var data = _context.ICUOMFactors.FromSqlRaw(sql).ToList<ICUOMFactors>();
            if (data != null && data.Count > 0)
            {
                if (data[0].ICUOMFactorMethodCombo == "Divide")
                {
                    if (data[0].ICUOMFactorQty > 0)
                        dbUOMFactor = 1 / data[0].ICUOMFactorQty;
                }
                else if (data[0].ICUOMFactorMethodCombo == "Multiply")
                {
                    dbUOMFactor = data[0].ICUOMFactorQty;
                }
            }
            else
            {
                sql = String.Format("Select * From ICUOMFactors Where FK_ICFromUOMID={0} And FK_ICToUOMID={1} AND AAStatus='Alive'", iICToUOMID, iICFromUOMID);
                data = _context.ICUOMFactors.FromSqlRaw(sql).ToList<ICUOMFactors>();
                if (data != null && data.Count > 0)
                {
                    if (data[0].ICUOMFactorMethodCombo == "Divide")
                    {
                        dbUOMFactor = data[0].ICUOMFactorQty;
                    }
                    else if (data[0].ICUOMFactorMethodCombo == "Multiply")
                    {
                        if (data[0].ICUOMFactorQty > 0)
                            dbUOMFactor = 1 / data[0].ICUOMFactorQty;
                    }
                }
            }

            return dbUOMFactor;
        }

        public ICProductUOMs GetProductUOMByProductUOM(int iProductID, int iUOMID)
        {
            string sql = string.Format(@"Select * from ICProductUOMs where AAStatus='Alive' AND FK_ICProductID={0}
                                                      AND FK_ICUOMID={1}", iProductID, iUOMID);
            return _context.ICProductUOMs.FromSqlRaw(sql).FirstOrDefault<ICProductUOMs>();
        }

        public JobTicketItemsKQSXInfo getItemsKQSXByLayout(int layoutId)
        {
            string sql = string.Format(@"select loi.PPMSLayoutItemID itemID, loi.PPMSLayoutItemQty qty, loi.PPMSLayoutItemDesc remark, icp.ICProductNo productNo,'Final' phaseName,icu.ICUOMName unit, CONVERT(decimal(4,2), 0) closed from PPMSLayoutItems loi 
inner join ICProducts icp on icp.ICProductID = loi.FK_ICProductID AND icp.AAStatus = 'Alive'
inner join ICUOMs icu on icu.ICUOMID = loi.FK_ICUOMID AND icu.AAStatus = 'Alive'
where  loi.PPMSLayoutItemID = {0} ", layoutId);
            return _context.JobTicketItemsKQSXInfo.FromSqlRaw(sql).FirstOrDefault<JobTicketItemsKQSXInfo>();
        }

        public int GetDefaultTranCfgIDByOrgTranCfgID(int piUserID, string psModName, int ipOrgTranCfg = 0)
        {
            var userModule = GetAllDataByCurrentUserAndModName(piUserID, psModName, ipOrgTranCfg);
            if (ipOrgTranCfg != 0 && userModule != null && userModule.Count > 0 && userModule.Count == 1)
            {
                return userModule[0].GLTranCfgID;
            }
            if (ipOrgTranCfg != 0)
                userModule = GetAllDataByCurrentUserAndModName(piUserID, psModName);
            if (userModule != null && userModule.Count > 0)
            {
                if (userModule.Count == 1)
                {
                    return userModule[0].GLTranCfgID;
                }
                else if (userModule.Count > 0)
                {
                    var selected = userModule.Where(u => u.GLTranCfgNoEditCheck == true).FirstOrDefault();
                    if (selected == null)
                        selected = userModule[0];
                    if (selected != null)
                    {
                        return selected.GLTranCfgID;
                    }
                }
            }
            return 0;
        }

        public List<GLTranCfgBasicInfo> GetAllDataByCurrentUserAndModName(int piUserID, string psModName, int ipOrgTranCfg = 0)
        {
            string query = string.Format(@"SELECT a.GLTranCfgID, a.GLTranCfgNoEditCheck
FROM GLTranCfgs a
INNER JOIN
  (SELECT GLTranCfgID
   FROM GLTranCfgs a
   INNER JOIN GLTranCfgUsers b ON a.AAStatus = 'Alive'
   AND a.GLTranCfgID = b.FK_GLTranCfgID
   AND (b.FK_ADUserID = {0}
        OR (b.FK_ADUserID = 0
            AND b.FK_GLUserGroupID IN
              (SELECT FK_GLUserGroupID
               FROM GLUserGroupItems
               WHERE AAStatus = 'Alive'
                 AND FK_ADUserID = {0} )))
   UNION SELECT GLTranCfgID
   FROM GLTranCfgs a
   LEFT JOIN GLTranCfgUsers b ON a.GLTranCfgID = b.FK_GLTranCfgID
   WHERE a.AAStatus = 'Alive'
     AND b.FK_GLTranCfgID IS NULL ) b ON a.GLTranCfgID = b.GLTranCfgID
INNER JOIN
  (SELECT GLTranCfgID
   FROM GLTranCfgs a
   INNER JOIN GLTranCfgGroups b ON a.FK_GLTranCfgGroupID = b.GLTranCfgGroupID
   INNER JOIN GLTranCfgGroupModules c ON b.GLTranCfgGroupID = c.FK_GLTranCfgGroupID   AND c.GLTranCfgGroupModuleName = N'{1}'
   WHERE GLTranCfgActiveCheck = 1 ) c ON a.GLTranCfgID = c.GLTranCfgID ", piUserID, psModName);
            if (ipOrgTranCfg > 0)
            {
                query += string.Format(@" LEFT JOIN (SELECT FK_GLTranCfgID
        FROM GLTranCfgSources
        WHERE FK_GLTranCfgSourceID = {0}
            AND AAStatus = 'Alive'
        GROUP BY FK_GLTranCfgID
        ) d ON a.GLTranCfgID = d.FK_GLTranCfgID
LEFT JOIN (SELECT FK_GLTranCfgID
        FROM GLTranCfgSources a
        WHERE AAStatus = 'Alive'
        GROUP BY FK_GLTranCfgID
        ) e ON a.GLTranCfgID = e.FK_GLTranCfgID ", ipOrgTranCfg);
                string strSQL = query + string.Format(@" WHERE d.FK_GLTranCfgID is NOT NULL");
                var rs = _context.GLTranCfgBasicInfo.FromSqlRaw(query).ToList<GLTranCfgBasicInfo>();
                if (rs == null || rs.Count == 0)
                {
                    strSQL = query + string.Format(@" WHERE d.FK_GLTranCfgID is NULL AND e.FK_GLTranCfgID IS NULL");
                    rs = _context.GLTranCfgBasicInfo.FromSqlRaw(query).ToList<GLTranCfgBasicInfo>();
                }
                return rs;
            }
            else
            {
                var rs = _context.GLTranCfgBasicInfo.FromSqlRaw(query).ToList<GLTranCfgBasicInfo>();
                return rs;
            }

        }

        public OnePropStringReturn getPositionNo(int positionID)
        {
            string sql = string.Format(@"select HRPositionNo dataReturn from HRPositions where HRPositionID = {0} ", positionID);
            return _context.OnePropStringReturn.FromSqlRaw(sql).FirstOrDefault<OnePropStringReturn>();
        }

        public List<OnePropIntReturn> getUserIdByName(List<string> userName)
        {
            string sql = string.Format(@"select ADUserID counts from ADUsers where ADUserName in ({0}) ",
                string.Join(", ", userName.Select(s => string.Format("'{0}'", s))));
            return _context.OnePropIntReturn.FromSqlRaw(sql).ToList<OnePropIntReturn>();
        }

        public List<CommentHistory> GetComment(string type, int id)
        {
            var tableName = ApproveType.ApproveTable[type];
            string sql = string.Format(@"select u.ADUserName createUser, ib.ADInboxItemDate createDate,
case when isnull(ib.ADInboxItemMessage,'') = '' then ib.ADInboxItemRemark else  ib.ADInboxItemMessage END content from ADInboxItems ib left join ADUsers u on ib.FK_ADFromUserID = u.ADUserID 
where ib.ADInboxItemObjectID = {0} AND ib.ADInboxItemAction = 'Comment' AND ib.ADInboxItemTableName = '{1}' ORDER BY ib.ADInboxItemDate", id, tableName);
            return _context.CommentHistory.FromSqlRaw(sql).ToList<CommentHistory>();
        }

        public List<ApproveHistory> GetApproveProcess(string type, int id)
        {
            var tableName = ApproveType.ApproveTable[type];
            var prefixColumns = tableName[0..^1];
            string sql = string.Format(@"
SELECT b.ADApprovalProcStepNo as approveNo
        ,CASE WHEN c.UserName IS NOT NULL 
            THEN
                CASE WHEN c.Action = 'Rejected' THEN 
                    N'Từ chối duyệt'
                WHEN c.Action = '' OR c.Action ='Submit' THEN 
                    N'Gởi phê duyệt'
                WHEN c.Action = 'Approved' THEN 
                    N'Đã phê duyệt'
                ELSE N''
                END
            ELSE N'' 
        END as approveStatus
        ,ISNULL(c.UserName,'') as approveUser
        ,b.AssignPositions approvePos
        ,c.Date as approveDate
        ,c.Remark as remark
FROM dbo.ADApprovalProcs a
    INNER JOIN dbo.ADApprovalProcSteps b ON a.ADApprovalProcID = b.FK_ADApprovalProcID
        AND a.AAStatus = 'Alive' AND b.AAStatus = 'Alive'
    INNER JOIN dbo.{2}s d ON a.ADApprovalProcID = d.FK_ADApprovalProcID 
        AND d.{2}ID = {0} AND d.AAStatus = 'Alive'
    LEFT JOIN dbo.ADDocHistorys c ON a.ADApprovalProcID = c.FK_ADApprovalProcID 
        AND ((LEN(b.AssignUsers) > 0 AND CHARINDEX(';' + c.UserName + ';',';' + b.AssignUsers) > 0)
            OR (LEN(b.AssignPositions) > 0 AND CHARINDEX(';' + c.PositionNo + ';',';' + b.AssignPositions) > 0)
            )
        AND ObjectID = {0}
        AND DocType = '{1}' 
        AND c.AAStatus = 'Alive'
ORDER BY CASE WHEN ADDocHistoryID is NULL THEN 1 ELSE 0 END, CASE WHEN ADDocHistoryID is NULL THEN ADApprovalProcStepLevel ELSE ADDocHistoryID END ",
    id, type, prefixColumns);

            return _context.ApproveHistory.FromSqlRaw(sql).ToList<ApproveHistory>();
        }

        public ADInboxItems GetLastApproveInboxItemByObjectId(int objectId, string tableName)
        {
            var prefixColumns = tableName[0..^1];
            var sql = string.Format(@"SELECT TOP 1 * FROM dbo.ADInboxItems Where AAStatus = 'Alive' AND ADInboxItemTableName = '{0}' AND ADInboxItemObjectID = {1} 
AND ADInboxItemProtocol = 'Approval' ORDER BY ADInboxItemID DESC", 
                tableName, objectId);
            return _context.ADInboxItems.FromSqlRaw(sql).FirstOrDefault<ADInboxItems>();
        }
        public ADInboxItems GetLastInboxItemByObjectId(int objectId, string tableName)
        {
            var prefixColumns = tableName[0..^1];
            var sql = string.Format(@"SELECT TOP 1 * FROM dbo.ADInboxItems Where AAStatus = 'Alive' AND ADInboxItemTableName = '{0}' AND ADInboxItemObjectID = {1}
ORDER BY ADInboxItemID DESC",
                tableName, objectId);
            return _context.ADInboxItems.FromSqlRaw(sql).FirstOrDefault<ADInboxItems>();
        }

        public int updateStatusApprove(string type, int objectId)
        {
            var tableName = ApproveType.ApproveTable[type];
            var prefixColumns = tableName[0..^1];
            var sql = string.Format(@"UPDATE {0} SET ApprovalStatusCombo = 'New' WHERE {1} = {2}",
                tableName, prefixColumns+"ID" ,objectId);
            return _context.Database.ExecuteSqlRaw(sql);
        }
    }
}
