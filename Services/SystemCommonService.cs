using AutoMapper;
using gmc_api.Base.dto;
using gmc_api.Base.dto.Product;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.HR;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using gmc_api.Repositories;
using System;
using System.Collections.Generic;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using static gmc_api.Base.Helpers.Constants;
using gmc_api.Base.Helpers;

namespace gmc_api.Services
{
    public interface ISystemCommonService : IServiceGMCBase<CommonInfo, DtoGMCBase, DtoGMCBase, DtoGMCBase>
    {
        IEnumerable<CommonInfo> GetDisplayData(string table);
        EmployeeBasicInfo getBasicEmployeeByUserId(int userId);
        List<EmployeeBasicInfo> getBasicEmployeeByListUserId(List<int> userIds);
        List<EmployeeBasicInfo> getBasicEmployeeByListEmpId(List<int> userIds);
        OnePropIntReturn CheckContainsApproveProcess(string type);
        ADInboxItemInfo GetDetailApproveProcess(string type, int objectId, string userName);
        int UpdateStatusMainObject(string type, int id, string status, int idApprovePro = -1);
        int UpdateStatusMailOuboxObject(string type, int id, string status);
        int GetOutBoxFromInBoxId(int id);
        ADOuboxItemInfo CovertInBoxToOutBox(ADInboxItemInfo approveData);
        ADDocHistoryInfo CovertInBoxToHistory(ADInboxItemInfo approveData, string userName, string position,int objectId);
        //ADOuboxItemInfo CovertInBoxToOutBox(ADInboxItemInfo approveData);
        List<ADInboxItemInfo> GetNextDetailApproveProcess(string type, int inboxId, int objectId, string aproveAction, string reasion, int procStep, string userName);
        public ADInboxItemInfo GetInboxItemById(int inboxId, int objectId, string type, string comment, UserLoginInfo userInfo,
            EmployeeBasicInfo empInfo);
        public string GetGeneralNoItem(Object mainObject, string module);

        public OnePropIntReturn CheckHaveNextObjectByRoutingIDAndPhase(int iRoutingID, int iPPPhaseCfgID);
        public OnePropStringReturn GetItemMSRevision(int iProductMSID, int iWOID);
        List<CommentHistory> GetComment(string type, int id);
        public OnePropIntReturn GetMSID(int iProductMSID, string msRevision);
        public ICProductBasicInfo GetBasicProductInfo(int productId);
        public decimal GetUOMFactor(int iICFromUOMID, int iICToUOMID);
        public ICProductUOMs GetProductUOMByProductUOM(int iProductID, int iUOMID);
        List<ApproveHistory> GetApproveProcess(string type, int id);
        public List<PPMSLayoutItemsInfo> GetListLayoutItemRevisionByWO(int iProductMSID, int iProductLayoutID, int iWOID);
        public void SetValuesAfterValidateProductQty(decimal pPProductionOrdrEstFGQty, PPProductionOrdrEstFGCreate prodFG, string tableName);
        public void SetValueAfterValidateUOM(int fK_ICUOMID, PPProductionOrdrEstFGCreate prodFG, string tableName);
        JobTicketItemsKQSXInfo getItemsKQSXByLayout(int pPMSLayoutItemID);
        public int GetDefaultTranCfgIDByOrgTranCfgID(int piUserID, string psModName, int ipOrgTranCfg = 0);
        string getPositionNo(int positionID);
        List<OnePropIntReturn> getUserIdByName(List<string> userName);
        int updateStatusApprove(string type, int objectId);
    }

    public class SystemCommonService : ServiceBaseImpl<CommonInfo, DtoGMCBase, DtoGMCBase, DtoGMCBase>, ISystemCommonService
    {
        private readonly ISystemCommonReponsitory _repository;
        private readonly IMapper _mapper;

        public SystemCommonService(ISystemCommonReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<CommonInfo> GetDisplayData(string table)
        {
            return _repository.GetDisplayData(table);
        }

        public EmployeeBasicInfo getBasicEmployeeByUserId(int userId)
        {
            return _repository.getBasicEmployeeByUserId(userId);
        }

        public OnePropIntReturn CheckContainsApproveProcess(string type)
        {
            return _repository.CheckContainsApproveProcess(type);
        }

        public ADInboxItemInfo GetDetailApproveProcess(string type, int objectId, string userName)
        {
            return _repository.GetDetailApproveProcess(type, objectId, userName);
        }

        public List<EmployeeBasicInfo> getBasicEmployeeByListEmpId(List<int> userIds)
        {
            return _repository.getBasicEmployeeByListEmpId(userIds);
        }

        public List<EmployeeBasicInfo> getBasicEmployeeByListUserId(List<int> userIds)
        {
            return _repository.getBasicEmployeeByListUserId(userIds);
        }

        public int UpdateStatusMainObject(string type, int id, string status, int idApprovePro = -1)
        {
            return _repository.UpdateStatusMainObject(ApproveType.ApproveTable[type], id, "ApprovalStatusCombo", status, idApprovePro);
        }

        public ADOuboxItemInfo CovertInBoxToOutBox(ADInboxItemInfo approveData)
        {
            return _mapper.Map<ADOuboxItemInfo>(approveData);
        }

        public List<ADInboxItemInfo> GetNextDetailApproveProcess(string type, int inboxId, int objectId, string aproveAction, string reasion, int procStep, string userName)
        {
            return _repository.GetNextDetailApproveProcess(type, inboxId, objectId, aproveAction, reasion, procStep, userName);
        }

        public int UpdateStatusMailOuboxObject(string type, int id, string status)
        {
            if (type == MailBoxType.INBOX)
                return _repository.UpdateStatusMainObject("ADInboxItems", id, "ADInboxItemAction", status);
            else
                return _repository.UpdateStatusMainObject("ADOutBoxItems", id, "ADOutboxItemAction", status);
        }

        public int GetOutBoxFromInBoxId(int id)
        {
            return _repository.GetOutBoxFromInBoxId(id);
        }

        public ADInboxItemInfo GetInboxItemById(int inboxId,int objectId, string type, string commnet, UserLoginInfo userInfo,
            EmployeeBasicInfo empInfo)
        {
            var data = new ADInboxItems();
            if (inboxId != 0)
                data = _repository.GetInboxItemById(inboxId);
            else 
            {
                data = _repository.GetLastApproveInboxItemByObjectId(objectId, ApproveType.ApproveTable[type]);
                if(data == null)
                {
                    data = _repository.GetLastInboxItemByObjectId(objectId, ApproveType.ApproveTable[type]);
                }
            }

            var dataObj = _repository.getBasicInfoObject(type, data.ADInboxItemObjectID);

            var userSubmit = new TwoPropReturn();
            if(inboxId != 0)
                userSubmit = _repository.getUserSubmit(inboxId);
            else
                userSubmit = _repository.getUserSubmitById(objectId, type);
            ADInboxItemInfo inboxData = _mapper.Map<ADInboxItemInfo>(data);
            inboxData.ADInboxItemMessage = commnet;
            inboxData.FK_ADFromUserID = userInfo.UserID;
            if (empInfo != null)
            {
                inboxData.ADInboxItemSubject = string.Format("{0} comment {1} {2}", empInfo.employeeName, ApproveType.ApproveTitle[type], dataObj.dataFirst);
                inboxData.FK_HRFromEmployeeID = empInfo.employeeId;
            }
            else
            {
                inboxData.ADInboxItemSubject = string.Format("{0} comment {1} {2}", userInfo.UserName, ApproveType.ApproveTitle[type], dataObj.dataFirst);
                inboxData.FK_HRFromEmployeeID = 0;
            }
            inboxData.ADInboxItemAction = string.Format("{0} comment {1} on {2}", userInfo.UserName, commnet, dataObj.dataFirst);
            inboxData.ADInboxItemDate = DateTime.Now;
            inboxData.ADInboxItemDocApprovalStatusCombo = "New";
            inboxData.ADMailToUsers += userSubmit.dataFirst + ";";
            inboxData.ADMailToUsers = inboxData.ADMailToUsers.Replace(userInfo.UserName + ";", "");
            //inboxData.ADInboxItemProtocol = MailBoxProtocolType.Message;
            return inboxData;
        }

        public string GetGeneralNoItem(Object mainObject, string module)
        {
            var existGLTranCfg = _repository.CheckContainColumn(TableModule.get[module], "FK_GLTranCfgID");

            string no = "";
            if (existGLTranCfg == null)
            {
                // DÙNG GEN MÃ
                var genNoItems = _repository.GetNoItemInfo(module);
                for (int i = 0; i < genNoItems.Count; i++)
                {
                    var genNo = genNoItems[i];
                    if (genNo.types == GenerateNo.TYPE_CONST)
                    {
                        no += genNo.st_type;
                    }
                    else if (genNo.types == GenerateNo.TYPE_COLUMNS)
                    {
                        if (!string.IsNullOrEmpty(genNo.cl_f_columnName))
                        {
                            var props = mainObject.GetType().GetProperty(genNo.cl_n_columnName);
                            if (props == null)
                            {
                                continue;
                            }
                            var rsValue = _repository.GetValueByColumn(genNo.cl_f_table, genNo.cl_f_columnName, (int)props.GetValue(mainObject));
                            if (rsValue == null)
                            {
                                continue;
                            }
                            no += rsValue.dataReturn;
                        }
                        else
                        {
                            var props = mainObject.GetType().GetProperty(genNo.cl_n_columnName);
                            if (props == null)
                            {
                                continue;
                            }
                            no += props.GetValue(mainObject).ToString();
                        }
                    }
                    else if (genNo.types == GenerateNo.TYPE_SYSTEMS)
                    {
                        if (genNo.st_type == GenerateNo.SYS_SEPER)
                        {
                            no += genNo.st_seper;
                        }
                        else if (genNo.st_type == GenerateNo.SYS_DATE)
                        {
                            no += DateTime.Now.ToString(genNo.st_date_format);
                        }
                        else if (genNo.st_type == GenerateNo.SYS_AUTO)
                        {
                            // Get No max id
                            var oNoMax = _repository.getNoWithMaxMapping(TableModule.get[module], no);
                            int iMaxID = 0;
                            if (oNoMax != null)
                            {
                                var currentCursor = oNoMax.dataReturn.Substring(no.Length, genNo.st_auto_length);
                                try
                                {
                                    iMaxID = Convert.ToInt32(currentCursor);
                                }
                                catch
                                {
                                    iMaxID = 0;
                                }
                            }

                            no += Convert.ToString(iMaxID + 1).PadLeft(genNo.st_auto_length, '0');
                        }
                    }
                }
            }
            else
            {
                var props = mainObject.GetType().GetProperty("FK_GLTranCfgID");
                if (props == null)
                {
                    return no;
                }
                var numberingInfo = _repository.getNumberingInfo((int)props.GetValue(mainObject));
                no += numberingInfo.first_preFix;// + numberingInfo.s
                if (!string.IsNullOrEmpty(numberingInfo.snd_dateFormat))
                {
                    no += DateTime.Now.ToString(numberingInfo.snd_dateFormat);
                }
                no += numberingInfo.third_separate.Trim();
                if (numberingInfo.four_autoLength != 0)
                {
                    var oNoMax = _repository.getNoWithMaxMapping(TableModule.get[module], no);
                    int iMaxID = 0;
                    if (oNoMax != null)
                    {
                        var currentCursor = oNoMax.dataReturn.Substring(no.Length, numberingInfo.four_autoLength);
                        try
                        {
                            iMaxID = Convert.ToInt32(currentCursor);
                        }
                        catch
                        {
                            iMaxID = 0;
                        }
                    }

                    no += Convert.ToString(iMaxID + 1).PadLeft(numberingInfo.four_autoLength, '0');
                }
            }
            return no;
        }

        public OnePropIntReturn CheckHaveNextObjectByRoutingIDAndPhase(int iRoutingID, int iPPPhaseCfgID)
        {
            return _repository.CheckHaveNextObjectByRoutingIDAndPhase(iRoutingID, iPPPhaseCfgID);
        }

        public OnePropStringReturn GetItemMSRevision(int iProductMSID, int iWOID)
        {
            throw new NotImplementedException();
        }

        public OnePropIntReturn GetMSID(int iProductMSID, string msRevision)
        {
            throw new NotImplementedException();
        }

        public ICProductBasicInfo GetBasicProductInfo(int productId)
        {
            return _repository.GetBasicProductInfo(productId);
        }

        public decimal GetUOMFactor(int iICFromUOMID, int iICToUOMID)
        {
            throw new NotImplementedException();
        }

        public ICProductUOMs GetProductUOMByProductUOM(int iProductID, int iUOMID)
        {
            throw new NotImplementedException();
        }

        public List<PPMSLayoutItemsInfo> GetListLayoutItemRevisionByWO(int iProductMSID, int iProductLayoutID, int iWOID)
        {
            return _repository.GetListLayoutItemRevisionByWO(iProductMSID, iProductLayoutID, iWOID);
        }

        public void SetValuesAfterValidateProductQty(decimal pPProductionOrdrEstFGQty, PPProductionOrdrEstFGCreate prodFG, string tableName)
        {
            String strColumnName = tableName.Substring(0, tableName.Length - 1) + ColumnSuffix.cstItemQtyColumnSuffix;
            Utils.SetPropertyValue(prodFG, strColumnName, pPProductionOrdrEstFGQty);

            QualityUtils.SetStockQty(prodFG, tableName);
            QualityUtils.SetItemWeightAndVolumn(prodFG, tableName, GetBasicProductInfo(prodFG.FK_ICProductID));
            //CalculateTotalAmt(MainObject, objItems, InvalidateLevel.None);
        }

        public void SetValueAfterValidateUOM(int fK_ICUOMID, PPProductionOrdrEstFGCreate prodFG, string tableName)
        {

            Utils.SetPropertyValue(prodFG, "FK_ICUOMID", fK_ICUOMID);

            int iICProductID = Convert.ToInt32(Utils.GetPropertyValue(prodFG, "FK_ICProductID"));
            var objICProductsInfo = GetBasicProductInfo(prodFG.FK_ICProductID);

            //Tinh lai Item Factor giua UOM va StkUOM
            String strColumnName = tableName.Substring(0, tableName.Length - 1) + ColumnSuffix.cstItemFactColumnSuffix;

            if (objICProductsInfo != null)
            {
                decimal dbItemFact = GetItemConversionFactor(objICProductsInfo, fK_ICUOMID);
                Utils.SetPropertyValue(prodFG, strColumnName, dbItemFact);
            }
            QualityUtils.SetStockQty(prodFG, tableName);
            QualityUtils.SetItemWeightAndVolumn(prodFG, tableName, objICProductsInfo);
        }

        public decimal GetItemConversionFactor(ICProductBasicInfo objICProductsInfo, int iICUOMID)
        {
            decimal dbFactor = 1;
            var ProductUOM = _repository.GetProductUOMByProductUOM(objICProductsInfo.ICProductID, iICUOMID);
            if (ProductUOM != null)
                dbFactor = ProductUOM.ICProductUOMFactor;
            else
                dbFactor = _repository.GetUOMFactor(iICUOMID, objICProductsInfo.FK_ICStkUOMID);

            return dbFactor;
        }

        public JobTicketItemsKQSXInfo getItemsKQSXByLayout(int layoutId)
        {
            return _repository.getItemsKQSXByLayout(layoutId);
        }

        public int GetDefaultTranCfgIDByOrgTranCfgID(int piUserID, string psModName, int ipOrgTranCfg = 0)
        {
            return _repository.GetDefaultTranCfgIDByOrgTranCfgID(piUserID, psModName, ipOrgTranCfg);
        }

        public ADDocHistoryInfo CovertInBoxToHistory(ADInboxItemInfo objADInboxItemInfo, string userName, string position,
            int objectId)
        {
            ADDocHistoryInfo DocHistory = new()
            {
                DocNo = objADInboxItemInfo.ADInboxItemDocNo,
                DocType = objADInboxItemInfo.ADInboxItemDocType,
                Remark = objADInboxItemInfo.ADInboxItemMessage,
                FK_ADApprovalProcID = objADInboxItemInfo.FK_ADApprovalProcID,
                FK_ADApprovalProdStepID = objADInboxItemInfo.FK_ADApprovalProcStepID,
                Action = objADInboxItemInfo.ADInboxItemAction,
                TableName = objADInboxItemInfo.ADInboxItemTableName,
                ADApprovalTypeCombo = "",
                ObjectID = objectId,// objADInboxItemInfo.ADInboxItemObjectID,
                UserName = userName,
                PositionNo = position,
                Date = DateTime.Now
            };
            return DocHistory;
        }

        public string getPositionNo(int positionID)
        {
            if(positionID == 0)
            {
                return "";
            }
            var data =  _repository.getPositionNo(positionID);
            if (data != null)
                return data.dataReturn;
            else
                return "";
        }

        public List<OnePropIntReturn> getUserIdByName(List<string> userName)
        {
            return _repository.getUserIdByName(userName);
        }

        public List<CommentHistory> GetComment(string type, int id)
        {
            return _repository.GetComment(type, id);
        }

        public List<ApproveHistory> GetApproveProcess(string type, int id)
        {
            return _repository.GetApproveProcess(type, id);
        }

        public int updateStatusApprove(string type, int objectId)
        {
            return _repository.updateStatusApprove(type, objectId);
        }
    }
}
