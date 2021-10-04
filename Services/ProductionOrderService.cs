using AutoMapper;
using gmc_api.DTO.FC;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using gmc_api.Repositories;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.Base.Helpers;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Services
{
    public interface IProductionOrderService : IServiceGMCBase<PPProductionOrdrResponse, PPProductionOrdrCreate, PPProductionOrdrs, PPProductionOrdrs>
    {
        //  LoginResponse Login(LoginRequest model);
        PPProductionOrdrGroupCount GetCountByGroupAll(string type);
        IEnumerable<ProductBasicInfoList> GetAll(string type, string status);
        JobTicketDetailBasic GetDetailJobTicketByNo(string no);
        List<JobTicketItemsInfo> GetListJobticketItems(int id);
        ProdRstDetailBasic GetDetailProdRstByNo(string no);
        List<ProdRstItemsInfo> GetListProdRstItems(int id);
        JobTicketDetail CombineJobTicketResult(JobTicketDetailBasic detail, List<JobTicketItemsInfo> items, IEnumerable<ADAttachmentReponse> attact, IEnumerable<ADCommentReponseCus> comment);
        JobTicketDetail CombineJobTicketResult(JobTicketDetailBasic detail, List<JobTicketItemsInfo> items, List<ADDocumentReponse> document);
        ProdRstDetail CombineProdRstResult(ProdRstDetailBasic detail, List<ProdRstItemsInfo> items, IEnumerable<ADAttachmentReponse> attact, IEnumerable<ADCommentReponseCus> comment);
        ProdRstDetail CreateTmpProdRst(JobTicketDetailBasic detail, List<JobTicketItemsKQSXInfo> items);
        List<ADCommentReponseCus> CovertComment(List<ADCommentReponse> comment);
        public List<JobTicketItemsKQSXInfo> GetListJobticketItemsKQSX(int id);
        List<PPProductionOrdrGroup> GetCountData(string type, ProductionOrderSearch data);
        List<ProductBasicInfoList> searchData(string type, ProductionOrderSearch data);
        ProdRstDetail CombineProdRstResult(ProdRstDetailBasic detail, List<ProdRstItemsInfo> items, List<ADDocumentReponse> conv);
    }

    public class ProductionOrderService : ServiceBaseImpl<PPProductionOrdrResponse, PPProductionOrdrCreate, PPProductionOrdrs, PPProductionOrdrs>, IProductionOrderService
    {

        private readonly AppSettings _appSettings;
        private readonly IProductionOrderReponsitory _repository;
        private readonly IMapper _mapper;

        public ProductionOrderService(IOptions<AppSettings> appSettings, IProductionOrderReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
            _mapper = mapper;
        }

        public JobTicketDetail CombineJobTicketResult(JobTicketDetailBasic detail, List<JobTicketItemsInfo> items, IEnumerable<ADAttachmentReponse> attact, IEnumerable<ADCommentReponseCus> comment)
        {
            var rs = _mapper.Map<JobTicketDetail>(detail);
            rs.detail = items;
            rs.attach = attact;
            rs.comment = comment;
            return rs;
        }

        public ProdRstDetail CombineProdRstResult(ProdRstDetailBasic detail, List<ProdRstItemsInfo> items, IEnumerable<ADAttachmentReponse> attact, IEnumerable<ADCommentReponseCus> comment)
        {
            var rs = _mapper.Map<ProdRstDetail>(detail);
            rs.detail = items;
            rs.attach = attact;
            rs.comment = comment;
            return rs;
        }

        public List<ADCommentReponseCus> CovertComment(List<ADCommentReponse> comment)
        {
            // var rs = comment.Select(u => _mapper.Map<ADCommentReponseCus>(u)).ToList(); 
            var rs = new List<ADCommentReponseCus>();
            for (int i = 0; i < comment.Count; i++)
            {
                var tmp = _mapper.Map<ADCommentReponseCus>(comment[i]);
                tmp.avatarUrl = "";
                rs.Add(tmp);
            }
            return rs;
        }

        public ProdRstDetail CreateTmpProdRst(JobTicketDetailBasic detail, List<JobTicketItemsKQSXInfo> items)
        {
            var rs = _mapper.Map<ProdRstDetail>(_mapper.Map<ProdRstDetailBasic>(detail));
            rs.detail = items.Select(u => _mapper.Map<ProdRstItemsInfo>(u)).ToList();
            rs.attach = new List<ADAttachmentReponse>();
            rs.comment = new List<ADCommentReponseCus>();
            rs.document = new List<ADDocumentReponse>();
            return rs;
        }

        public IEnumerable<ProductBasicInfoList> GetAll(string type, string status)
        {
            var sqlCondition = "";
            if (type == ProductType.JOB_TIKCET)
                sqlCondition += " po.PPProductionOrdrTypeCombo = 'ProductionOrdr'";
            else if (type == ProductType.P_RESULT)
            {
                sqlCondition += " po.PPProductionOrdrTypeCombo = 'ProductionFG'";
                return _repository.GetProductList(sqlCondition);
            }
            if (status == ProductOrderStatus.NEW)
            {
                sqlCondition += " AND po.PPProductionOrdrStatus = 'New' AND isnull(po.PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23)";
            }
            else if (status == ProductOrderStatus.TRANSFERD)
            {
                sqlCondition += " AND po.PPProductionOrdrStatus = 'Transfered'";
            }
            else if (status == ProductOrderStatus.TRANSFERING)
            {
                sqlCondition += " AND po.PPProductionOrdrStatus = 'Transfering'";
            }
            else if (status == ProductOrderStatus.OVERDUE)
            {
                sqlCondition += " AND po.PPProductionOrdrStatus = 'New' AND isnull(po.PPProductionOrdrEstEndDate,getdate()) <  CONVERT(VARCHAR(10), getdate(), 23)";
            }


            return _repository.GetProductList(sqlCondition);
        }

        public PPProductionOrdrGroupCount GetCountByGroupAll(string type)
        {
            var sqlCondition = "";
            if (type == ProductType.JOB_TIKCET)
                sqlCondition += " PPProductionOrdrTypeCombo = 'ProductionOrdr'";
            else if (type == ProductType.P_RESULT)
                sqlCondition += " PPProductionOrdrTypeCombo = 'ProductionFG'";
            return _repository.GetCountByGroupAll(sqlCondition);
        }

        public List<PPProductionOrdrGroup> GetCountData(string type, ProductionOrderSearch data)
        {
            initData(data);
            var sqlSelete = " count(poef.PPProductionordrID) counts,";
            var sqlCondition = " PPProductionOrdrStatus IN ('New','Transfering','Transfered') ";
            var joinCondition = " PPProductionordrs poef ";
            var grouByCondition = "";
            if (type == ProductType.JOB_TIKCET)
                sqlCondition += " AND poef.AAStatus = 'Alive' AND poef.PPProductionOrdrTypeCombo = 'ProductionOrdr'";
            else if (type == ProductType.P_RESULT)
                sqlCondition += " AND poef.AAStatus = 'Alive' AND poef.PPProductionOrdrTypeCombo = 'ProductionFG'";
            if(data.jobTicket != Constants.DEFAULT_VALUE_STRING
                && type == ProductType.P_RESULT)
            {
                joinCondition += string.Format(@" inner join PPProductionordrs poef1 on poef1.PPProductionordrID = poef.FK_PPProductionOrdrParentID and poef1.PPProductionOrdrNo like N'{0}%' ",
                    data.jobTicket);
            }
            if (data.phase != Constants.DEFAULT_VALUE_STRING)
            {
                joinCondition += string.Format(@" inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID and phase.PPPhaseCfgName like N'{0}%' ",
                   data.phase);
            }
            if (data.status != Constants.DEFAULT_VALUE_STRING)
            {
                if(data.status == "Open")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'New' AND isnull(poef.PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23) ";
                } else if (data.status == "Incomplete")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'Transfering' ";
                }
                else if (data.status == "Complete")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'Transfered' ";
                }
                else if (data.status == "Overdue")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'New' AND isnull(poef.PPProductionOrdrEstEndDate,getdate()) <  CONVERT(VARCHAR(10), getdate(), 23) ";
                }
            }
            if (data.workCenter != Constants.DEFAULT_VALUE_STRING)
            {
                joinCondition += string.Format(@" inner JOIN PPWorkCenters wc ON poef.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive' and wc.PPWorkCenterName like N'{0}%' ",
                   data.workCenter);
            }
            if (data.workOrder != Constants.DEFAULT_VALUE_STRING)
            {
                joinCondition += string.Format(@" inner JOIN PPWOs wo ON poef.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive' and wo.PPWONo like N'{0}%' ",
                   data.workOrder);
            }
            if (data.createDate != Constants.DEFAULT_VALUE_STRING)
            {
                sqlCondition += string.Format(" AND  poef.PPProductionOrdrDate ='{0}' ", data.createDate);
            }
            if(data.groupByColumn == Constants.DEFAULT_VALUE_STRING
                || data.groupByColumn == "status")
            {
                //sqlCondition += string.Format( " ");
                sqlSelete += string.Format(@" case WHEN PPProductionOrdrStatus = 'New' AND isnull(PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23) then 'Open'
 when PPProductionOrdrStatus = 'Transfering' then 'Incomplete'
  when PPProductionOrdrStatus = 'Transfered' then 'Complete'
 when PPProductionOrdrStatus = 'New' AND isnull(PPProductionOrdrEstEndDate,getdate()) <  CONVERT(VARCHAR(10), getdate(), 23) then 'Overdue' ELSE 'Others' end AS name");
                grouByCondition = string.Format(@" GROUP BY case WHEN PPProductionOrdrStatus = 'New' AND isnull(PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23) then 'Open'
 when PPProductionOrdrStatus = 'Transfering' then 'Incomplete'
  when PPProductionOrdrStatus = 'Transfered' then 'Complete'
 when PPProductionOrdrStatus = 'New' AND isnull(PPProductionOrdrEstEndDate, getdate()) < CONVERT(VARCHAR(10), getdate(), 23) then 'Overdue' ELSE 'Others' end");
            } else if(data.groupByColumn == "phase")
            {
                sqlSelete += " phase.PPPhaseCfgName name ";
                grouByCondition = " GROUP BY phase.PPPhaseCfgName";
                if (data.phase == Constants.DEFAULT_VALUE_STRING)
                {
                    joinCondition += string.Format(@" inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID ");
                }
            }
            else if (data.groupByColumn == "workOrder")
            {
                sqlSelete += " isnull(case wo.PPWONo WHEN '' THEN NULL ELSE wo.PPWONo END , 'Others') name ";
                grouByCondition = " GROUP BY wo.PPWONo";
                if (data.workOrder == Constants.DEFAULT_VALUE_STRING)
                {
                    joinCondition += string.Format(@" left JOIN PPWOs wo ON poef.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive' ");
                }
            }
            else if (data.groupByColumn == "workCenter")
            {
                sqlSelete += " isnull(case wc.PPWorkCenterName WHEN '' THEN NULL ELSE wc.PPWorkCenterName END , 'Others') name ";
                grouByCondition = " GROUP BY wc.PPWorkCenterName ";
                if (data.workCenter == Constants.DEFAULT_VALUE_STRING)
                {
                    joinCondition += string.Format(@" left JOIN PPWorkCenters wc ON poef.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive' ");
                }
            }
            else if (data.groupByColumn == "createDate")
            {
                sqlSelete += " CONVERT(VARCHAR(10), poef.PPProductionOrdrDate, 23) name ";
                grouByCondition = " GROUP BY CONVERT(VARCHAR(10), poef.PPProductionOrdrDate, 23) ";
            }
            var sql = string.Format(@"SELECT {0} FROM {1} WHERE {2} {3} ", sqlSelete, joinCondition, sqlCondition, grouByCondition);
            return _repository.GetCount(sql);
        }

        public JobTicketDetailBasic GetDetailJobTicketByNo(string no)
        {
            return _repository.GetDetailJobTicketByNo(no);
        }

        public ProdRstDetailBasic GetDetailProdRstByNo(string no)
        {
            return _repository.GetDetailProdRstByNo(no);
        }

        public List<JobTicketItemsInfo> GetListJobticketItems(int id)
        {
            return _repository.GetListJobticketItems(id);
        }

        public List<JobTicketItemsKQSXInfo> GetListJobticketItemsKQSX(int id)
        {
            return _repository.GetListJobticketItemsKQSX(id);
        }

        public List<ProdRstItemsInfo> GetListProdRstItems(int id)
        {
            return _repository.GetListProdRstItems(id);
        }

        public List<ProductBasicInfoList> searchData(string type, ProductionOrderSearch data)
        {
            initData(data);
            var sqlSelete = string.Format(@"select poef.PPProductionOrdrNo pOrdNo, poef.PPProductionOrdrDate pOrdDate, phase.PPPhaseCfgName phaseName
from PPProductionordrs poef inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID {0} ",
data.phase != Constants.DEFAULT_VALUE_STRING ? string.Format(@" and phase.PPPhaseCfgName like N'{0}%'", data.phase) : "");
            var sqlCondition = " poef.PPProductionOrdrStatus IN ('New','Transfering','Transfered') ";
            var joinCondition = " ";
            if (type == ProductType.JOB_TIKCET)
                sqlCondition += " AND poef.AAStatus = 'Alive' AND poef.PPProductionOrdrTypeCombo = 'ProductionOrdr'";
            else if (type == ProductType.P_RESULT)
                sqlCondition += " AND poef.AAStatus = 'Alive' AND poef.PPProductionOrdrTypeCombo = 'ProductionFG'";
            if (data.jobTicket != Constants.DEFAULT_VALUE_STRING
                && type == ProductType.P_RESULT)
            {
                joinCondition += string.Format(@" inner join PPProductionordrs poef1 on poef1.PPProductionordrID = poef.FK_PPProductionOrdrParentID and poef1.PPProductionOrdrNo like N'{0}%' ",
                    data.jobTicket);
            }
            if (data.status != Constants.DEFAULT_VALUE_STRING)
            {
                if (data.status == "Open") //'Incomplete' Overdue  Others Open
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'New' AND isnull(poef.PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23) ";
                }
                else if (data.status == "Incomplete")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'Transfering' ";
                }
                else if (data.status == "Complete")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'Transfered' ";
                }
                else if (data.status == "Overdue")
                {
                    sqlCondition += " AND poef.PPProductionOrdrStatus = 'New' AND isnull(poef.PPProductionOrdrEstEndDate,getdate()) <  CONVERT(VARCHAR(10), getdate(), 23) ";
                }
            }
            if (data.workCenter != Constants.DEFAULT_VALUE_STRING)
            {

                if (data.workCenter == "Others")
                {
                    sqlCondition += " AND isnull(poef.FK_PPWorkCenterID,'') = ''  ";
                  //  joinCondition += string.Format(@" left JOIN PPWorkCenters wc ON poef.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive' AND isnull(wc.PPWorkCenterName,'') = '' ");
                } else
                {
                    joinCondition += string.Format(@" inner JOIN PPWorkCenters wc ON poef.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive' and wc.PPWorkCenterName like N'{0}%' ",
                       data.workCenter);
                }
            }
            if (data.workOrder != Constants.DEFAULT_VALUE_STRING)
            {
                if (data.workOrder == "Others")
                {
                    sqlCondition += " AND isnull(poef.FK_PPWOID,'') = '' ";
                   // joinCondition += string.Format(@" left JOIN PPWOs wo ON poef.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive' AND isnull(wo.PPWONo, '') = '' ");
                }
                else
                {
                    joinCondition += string.Format(@" inner JOIN PPWOs wo ON poef.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive' and wo.PPWONo like N'{0}%' ",
                   data.workOrder);
                }
            }
            if (data.createDate != Constants.DEFAULT_VALUE_STRING)
            {
                sqlCondition += string.Format(" AND  poef.PPProductionOrdrDate ='{0}' ", data.createDate);
            }

            var sql = string.Format(@"{0} {1} WHERE {2} ", sqlSelete, joinCondition, sqlCondition);
            return _repository.searchData(sql);
        }

        private static void initData(ProductionOrderSearch data)
        {
            if (data.createDate == null)
            {
                data.createDate = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.groupByColumn.Trim().Length == 0)
            {
                data.groupByColumn = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.jobTicket.Trim().Length == 0)
            {
                data.jobTicket = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.phase.Trim().Length == 0)
            {
                data.phase = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.status.Trim().Length == 0)
            {
                data.status = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.workCenter.Trim().Length == 0)
            {
                data.workCenter = Constants.DEFAULT_VALUE_STRING;
            }
            if (data.workOrder.Trim().Length == 0)
            {
                data.workOrder = Constants.DEFAULT_VALUE_STRING;
            }
        }

        public JobTicketDetail CombineJobTicketResult(JobTicketDetailBasic detail, List<JobTicketItemsInfo> items, List<ADDocumentReponse> document)
        {
            var rs = _mapper.Map<JobTicketDetail>(detail);
            rs.detail = items;
            rs.document = document ?? new List<ADDocumentReponse>();
            return rs;
        }

        public ProdRstDetail CombineProdRstResult(ProdRstDetailBasic detail, List<ProdRstItemsInfo> items, List<ADDocumentReponse> conv)
        {
            var rs = _mapper.Map<ProdRstDetail>(detail);
            rs.detail = items;
            rs.document = conv ?? new List<ADDocumentReponse>();
            return rs;
        }
    }
}
