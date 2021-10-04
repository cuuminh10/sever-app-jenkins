using AutoMapper;
using gmc_api.Base.dto;
using gmc_api.DTO.AR;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.Payment;
using gmc_api.DTO.PO;
using gmc_api.DTO.PR;
using gmc_api.Repositories;
using System.Collections.Generic;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using static gmc_api.Base.Helpers.Constants;
using System.Linq;

namespace gmc_api.Services
{
    public interface ISimpleService : IServiceGMCBase<CommonInfo, DtoGMCBase, DtoGMCBase, DtoGMCBase>
    {
        public List<APPRInfo> getAPPRMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public List<APPRInfo> getAPPRApproveData(string userName, ApproveSearchBase consdition);
        public List<APPRItemInfo> getDetailAPPRItems(int parrentId);

        public List<APPOInfo> getAPPOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public List<APPOInfo> getAPPOApproveData(string userName, ApproveSearchBase consdition);
        public List<APPOItemInfo> getDetailAPPOItems(int parrentId);

        public List<ARSOItemInfo> getDetailSOItems(int parrentId);
        public List<ARSOInfo> getSOApproveData(string userName, ApproveSearchBase consdition);
        public List<ARSOInfo> getSOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);

        public List<GLVoucherPaymentItemInfo> getDetailVoucherPaymentItems(int parrentId);
        public List<GLVoucherPaymentInfo> getVoucherPaymentApproveData(string userName, ApproveSearchBase consdition);
        public List<GLVoucherPaymentInfo> getVoucherPaymentMyData(string userName, ApproveSearchBase consdition, int employeeId = -1);
        public AproveChart collectionData(string userName, int employeeId, int type, string objType);
    }
    public class SimpleService : ServiceBaseImpl<CommonInfo, DtoGMCBase, DtoGMCBase, DtoGMCBase>, ISimpleService
    {
        private readonly ISimpleReponsitory _repository;
        private readonly IMapper _mapper;

        public SimpleService(ISimpleReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public AproveChart collectionData(string userName, int employeeId, int type, string objType)
        {
            return _repository.collectionData(userName, employeeId, type, objType);
        }

        public List<APPOInfo> getAPPOApproveData(string userName, ApproveSearchBase consdition)
        {
            var data = _repository.getAPPOApproveData(userName, consdition);
            if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED)
            {
                foreach (APPOInfo item in data)
                {
                    if(item.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || item.ApprovalStatusCombo == ApproveStatus.INPROCESS)
                    {
                        // Check next step conffirm?
                        var isDisplay = _repository.checkNextLevelApprove(item.APPOID, item.ADInboxItemID, "APPOs");
                        if(isDisplay == null || isDisplay.counts == 0)
                        {
                            item.displayReject = 1;
                        }
                    }
                    else if ( item.ApprovalStatusCombo == ApproveStatus.APROVED)
                    {
                        // Check user last
                        var lastApprove = _repository.getLastAproveLevel(item.FK_ADApprovalProcStepID);
                        if (lastApprove == null)
                        {
                            continue;
                        }
                        if (lastApprove.dataFirst.Replace(";", "") == userName)
                        {
                            item.displayReject = 1;
                            continue;
                        }
                        if (lastApprove.dataSecond != null && lastApprove.dataSecond.Trim().Length > 0)
                        {
                            var tmpUserPosition = lastApprove.dataSecond.Split(";").ToList<string>();
                            tmpUserPosition = tmpUserPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                            var positionList = _repository.getUserByPosition(tmpUserPosition);
                            if (positionList == null || positionList.Count == 0)
                            {
                                continue;
                            }
                            for (int i = 0; i < positionList.Count; i++)
                            {
                                if (positionList[i].dataReturn == userName)
                                {
                                    item.displayReject = 1;
                                    break;
                                }
                            }

                        }
                    }
                };
            }
            return data;
        }

        public List<APPOInfo> getAPPOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            return _repository.getAPPOMyData(userName, consdition, employeeId);
        }

        public List<APPRInfo> getAPPRApproveData(string userName, ApproveSearchBase consdition)
        {
            var data = _repository.getAPPRApproveData(userName, consdition);
            if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED)
            {
                foreach(APPRInfo item in data)
                {
                    if (item.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || item.ApprovalStatusCombo == ApproveStatus.INPROCESS)
                    {
                        // Check next step conffirm?
                        var isDisplay = _repository.checkNextLevelApprove(item.APPRID, item.ADInboxItemID, "APPRs");
                        if (isDisplay == null || isDisplay.counts == 0)
                        {
                            item.displayReject = 1;
                        }
                    }
                    else if ( item.ApprovalStatusCombo == ApproveStatus.APROVED)
                    {
                        // Check user last
                        var lastApprove = _repository.getLastAproveLevel(item.FK_ADApprovalProcStepID);
                        if (lastApprove == null)
                        {
                            continue;
                        }
                        if (lastApprove.dataFirst.Replace(";", "") == userName)
                        {
                            item.displayReject = 1;
                            continue;
                        }
                        if (lastApprove.dataSecond != null && lastApprove.dataSecond.Trim().Length > 0)
                        {
                            var tmpUserPosition = lastApprove.dataSecond.Split(";").ToList<string>();
                            tmpUserPosition = tmpUserPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                            var positionList = _repository.getUserByPosition(tmpUserPosition);
                            if (positionList == null || positionList.Count == 0)
                            {
                                continue;
                            }
                            for (int i = 0; i < positionList.Count; i++)
                            {
                                if (positionList[i].dataReturn == userName)
                                {
                                    item.displayReject = 1;
                                    break;
                                }
                            }

                        }
                    }
                };
            }
            return data;
        }

        public List<APPRInfo> getAPPRMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            return _repository.getAPPRMyData(userName, consdition, employeeId);
        }

        public List<APPOItemInfo> getDetailAPPOItems(int parrentId)
        {
            return _repository.getDetailAPPOItems(parrentId);
        }

        public List<APPRItemInfo> getDetailAPPRItems(int parrentId)
        {
            return _repository.getDetailAPPRItems(parrentId);
        }

        public List<ARSOItemInfo> getDetailSOItems(int parrentId)
        {
            return _repository.getDetailSOItems(parrentId);
        }

        public List<GLVoucherPaymentItemInfo> getDetailVoucherPaymentItems(int parrentId)
        {
            return _repository.getDetailVoucherPaymentItems(parrentId);
        }

        public List<ARSOInfo> getSOApproveData(string userName, ApproveSearchBase consdition)
        {
            var data = _repository.getSOApproveData(userName, consdition);
            if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED)
            {
                foreach(ARSOInfo item in data)
                {
                    if (item.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || item.ApprovalStatusCombo == ApproveStatus.INPROCESS)
                    {
                        // Check next step conffirm?
                        var isDisplay = _repository.checkNextLevelApprove(item.ARSOID, item.ADInboxItemID, "ARSOs");
                        if (isDisplay == null || isDisplay.counts == 0)
                        {
                            item.displayReject = 1;
                        }
                    }
                    else if (item.ApprovalStatusCombo == ApproveStatus.APROVED)
                    {
                        // Check user last
                        var lastApprove = _repository.getLastAproveLevel(item.FK_ADApprovalProcStepID);
                        if (lastApprove == null)
                        {
                            continue;
                        }
                        if (lastApprove.dataFirst.Replace(";", "") == userName)
                        {
                            item.displayReject = 1;
                            continue;
                        }
                        if (lastApprove.dataSecond != null && lastApprove.dataSecond.Trim().Length > 0)
                        {
                            var tmpUserPosition = lastApprove.dataSecond.Split(";").ToList<string>();
                            tmpUserPosition = tmpUserPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                            var positionList = _repository.getUserByPosition(tmpUserPosition);
                            if (positionList == null || positionList.Count == 0)
                            {
                                continue;
                            }
                            for (int i = 0; i < positionList.Count; i++)
                            {
                                if (positionList[i].dataReturn == userName)
                                {
                                    item.displayReject = 1;
                                    break;
                                }
                            }

                        }
                    }
                };
            }
            return data;
        }

        public List<ARSOInfo> getSOMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            return _repository.getSOMyData(userName, consdition, employeeId);
        }

        public List<GLVoucherPaymentInfo> getVoucherPaymentApproveData(string userName, ApproveSearchBase consdition)
        {
            var data = _repository.getVoucherPaymentApproveData(userName, consdition);
            if (consdition.ApprovalStatusCombo == ApproveStatus.APROVED)
            {
                foreach(GLVoucherPaymentInfo item in data)
                {
                    if (item.ApprovalStatusCombo == ApproveStatus.APROVVING
                    || item.ApprovalStatusCombo == ApproveStatus.INPROCESS)
                    {
                        // Check next step conffirm?
                        var isDisplay = _repository.checkNextLevelApprove(item.GLVoucherID, item.ADInboxItemID, "GLVouchers");
                        if (isDisplay == null || isDisplay.counts == 0)
                        {
                            item.displayReject = 1;
                        }
                    } else if (item.ApprovalStatusCombo == ApproveStatus.APROVED)
                    {
                        // Check user last
                        var lastApprove = _repository.getLastAproveLevel(item.FK_ADApprovalProcStepID);
                        if (lastApprove == null)
                        {
                            continue;
                        }
                        if (lastApprove.dataFirst.Replace(";", "") == userName)
                        {
                            item.displayReject = 1;
                            continue;
                        }
                        if (lastApprove.dataSecond != null && lastApprove.dataSecond.Trim().Length > 0)
                        {
                            var tmpUserPosition = lastApprove.dataSecond.Split(";").ToList<string>();
                            tmpUserPosition = tmpUserPosition.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                            var positionList = _repository.getUserByPosition(tmpUserPosition);
                            if (positionList == null || positionList.Count == 0)
                            {
                                continue;
                            }
                            for (int i = 0; i < positionList.Count; i++)
                            {
                                if (positionList[i].dataReturn == userName)
                                {
                                    item.displayReject = 1;
                                    break;
                                }
                            }

                        }
                    }
                };
            }
            return data;
        }

        public List<GLVoucherPaymentInfo> getVoucherPaymentMyData(string userName, ApproveSearchBase consdition, int employeeId = -1)
        {
            return _repository.getVoucherPaymentMyData(userName, consdition, employeeId);
        }
    }
}
