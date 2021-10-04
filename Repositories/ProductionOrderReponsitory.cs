using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Repositories
{
    public interface IProductionOrderReponsitory : IRepositoriesBase<PPProductionOrdrs, PPProductionOrdrs>
    {
        // Entities.User GetUserByNameAndPass(string UserName, string Password);
        PPProductionOrdrGroupCount GetCountByGroupAll(string sqlCondition);
        IEnumerable<ProductBasicInfoList> GetProductList(string sqlCondition);
        JobTicketDetailBasic GetDetailJobTicketByNo(string no);
        List<JobTicketItemsInfo> GetListJobticketItems(int id);
        public List<JobTicketItemsKQSXInfo> GetListJobticketItemsKQSX(int id);
        ProdRstDetailBasic GetDetailProdRstByNo(string no);
        List<ProdRstItemsInfo> GetListProdRstItems(int id);
        List<PPProductionOrdrGroup> GetCount(string sql);
        List<ProductBasicInfoList> searchData(string sql);
    }

    public class ProductionOrderReponsitory : RepositoriesBaseImpl<PPProductionOrdrs, PPProductionOrdrs>, IProductionOrderReponsitory
    {
        private readonly GMCContext _context;
        public ProductionOrderReponsitory(GMCContext context) : base(context, "PPProductionOrdrs", "PPProductionOrdrID")
        {
            _context = context;
        }

        public List<PPProductionOrdrGroup> GetCount(string sql)
        {
            return _context.PPProductionOrdrGroup.FromSqlRaw(sql).ToList<PPProductionOrdrGroup>();
        }

        public PPProductionOrdrGroupCount GetCountByGroupAll(string sqlCondition)
        {
            var sqlBuilding = String.Format(@"select
	isnull(sum( case when PPProductionOrdrStatus = 'New' AND isnull(PPProductionOrdrEstEndDate,getdate()) >=  CONVERT(VARCHAR(10), getdate(), 23) then 1 else 0 end ),0) opens,
	isnull(sum( case when PPProductionOrdrStatus = 'Transfering' then 1 else 0 end ),0) incompleted,
	isnull(sum( case when PPProductionOrdrStatus = 'Transfered'  then 1 else 0 end ),0)  completed,
	isnull(sum( case when PPProductionOrdrStatus = 'New' AND  isnull(PPProductionOrdrEstEndDate,getdate()) <  CONVERT(VARCHAR(10), getdate(), 23) then 1 else 0 end ),0) overdue
from PPProductionordrs where AAStatus = 'Alive'  AND {0} ", sqlCondition);
            return _context.PPProductionOrdrGroupCount.FromSqlRaw(sqlBuilding).Single<PPProductionOrdrGroupCount>();
        }

        public JobTicketDetailBasic GetDetailJobTicketByNo(string no)
        {
            var sqlBuilding = String.Format(@"select pdo.PPProductionOrdrID id, pdo.PPProductionOrdrNo no, pdo.PPProductionOrdrName name,pdo.PPProductionOrdrDesc description,
pdo.PPProductionOrdrDate ordDate, hr.HREmployeeName employeeName, wo.PPWONo woNo, wc.PPWorkCenterName workCenterName, phase.PPPhaseCfgNo phaseNo
from PPProductionOrdrs pdo 
inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = pdo.FK_PPPhaseCfgID 
left JOIN PPWOs wo ON pdo.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive'
left JOIN PPWorkCenters wc ON pdo.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive'
LEFT JOIN HREmployees hr on pdo.FK_HREmployeeID = hr.HREmployeeID
where pdo.AAStatus = 'Alive' and pdo.PPProductionOrdrNo = '{0}' AND pdo.PPProductionOrdrTypeCombo = 'ProductionOrdr'", no);
            return _context.JobTicketDetailBasic.FromSqlRaw(sqlBuilding).FirstOrDefault<JobTicketDetailBasic>();
        }

        public ProdRstDetailBasic GetDetailProdRstByNo(string no)
        {
            var sqlBuilding = String.Format(@"select pdo.PPProductionOrdrID id, pdo.PPProductionOrdrNo no, pdo.PPProductionOrdrName name,pdo.PPProductionOrdrDesc description,
pdo.PPProductionOrdrDate ordDate, hr.HREmployeeName employeeName, wo.PPWONo woNo, pdo2.PPProductionOrdrNo jobTicketNo,
wc.PPWorkCenterName workCenterName, phase.PPPhaseCfgNo phaseNo
from PPProductionOrdrs pdo 
inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = pdo.FK_PPPhaseCfgID 
LEFT JOIN PPWOs wo ON pdo.FK_PPWOID = wo.PPWOID and wo.AAStatus = 'Alive'
left JOIN PPWorkCenters wc ON pdo.FK_PPWorkCenterID = wc.PPWorkCenterID and wc.AAStatus = 'Alive'
LEFT JOIN HREmployees hr on pdo.FK_HREmployeeID = hr.HREmployeeID
LEFT JOIN PPProductionOrdrs pdo2 on pdo.FK_PPProductionOrdrParentID = pdo2.PPProductionOrdrID
where pdo.AAStatus = 'Alive' and pdo.PPProductionOrdrNo = '{0}' AND pdo.PPProductionOrdrTypeCombo = 'ProductionFG'", no);
            return _context.ProdRstDetailBasic.FromSqlRaw(sqlBuilding).FirstOrDefault<ProdRstDetailBasic>();
        }

        public List<JobTicketItemsInfo> GetListJobticketItems(int id)
        {
            var sqlBuilding = String.Format(@"select poef.PPProductionOrdrEstFGID itemId,poef.PPProductionOrdrEstFGQty qty,  poef.PPProductionOrdrEstFGDesc remark, icp.ICProductNo productNo,
icp.ICProductName productName, phase.PPPhaseCfgName phaseName, icu.ICUOMName unit 
from PPProductionOrdrEstFGs poef inner join ICProducts icp on icp.ICProductID = poef.FK_ICProductID  inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID 
inner join ICUOMs icu on icu.ICUOMID = poef.FK_ICUOMID where poef.FK_PPProductionOrdrID = {0} AND poef.AAStatus = 'Alive'", id);
            return _context.JobTicketItemsInfo.FromSqlRaw(sqlBuilding).ToList<JobTicketItemsInfo>();
        }

        public List<JobTicketItemsKQSXInfo> GetListJobticketItemsKQSX(int id)
        {
            var sqlBuilding = String.Format(@"select poef.PPProductionOrdrEstFGID itemId,poef.PPProductionOrdrEstFGRQty qty,  poef.PPProductionOrdrEstFGCloseQty closed, poef.PPProductionOrdrEstFGDesc remark, icp.ICProductNo productNo, icp.ICProductName productName, phase.PPPhaseCfgName phaseName, icu.ICUOMName unit
from PPProductionOrdrEstFGs poef inner join ICProducts icp on icp.ICProductID = poef.FK_ICProductID  inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID 
inner join ICUOMs icu on icu.ICUOMID = poef.FK_ICUOMID where poef.FK_PPProductionOrdrID = {0} AND poef.PPProductionOrdrEstFGRQty > 0 AND poef.AAStatus = 'Alive'", id);
            return _context.JobTicketItemsKQSXInfo.FromSqlRaw(sqlBuilding).ToList<JobTicketItemsKQSXInfo>();
        }

        public List<ProdRstItemsInfo> GetListProdRstItems(int id)
        {
            var sqlBuilding = String.Format(@"select poef.PPProductionOrdrEstFGID itemId,
poef.PPProductionOrdrEstFGOrgQty qty, 
poef.PPProductionOrdrEstFGSetupQty setUpQty, 
poef.PPProductionOrdrEstFGCQty cancelQty, 
poef.PPProductionOrdrEstFGNCRQty NCRQty, 
poef.PPProductionOrdrEstFGDesc remark, icp.ICProductNo productNo, 
icp.ICProductName productName, phase.PPPhaseCfgName phaseName, icu.ICUOMName unit 
from PPProductionOrdrEstFGs poef
inner join ICProducts icp on icp.ICProductID = poef.FK_ICProductID 
inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = poef.FK_PPPhaseCfgID 
inner join ICUOMs icu on icu.ICUOMID = poef.FK_ICUOMID 
where poef.FK_PPProductionOrdrID = {0} AND poef.AAStatus = 'Alive'", id);
            return _context.ProdRstItemsInfo.FromSqlRaw(sqlBuilding).ToList<ProdRstItemsInfo>();
        }

        public IEnumerable<ProductBasicInfoList> GetProductList(string sqlCondition)
        {
            var sqlBuilding = String.Format(@"select po.PPProductionOrdrNo pOrdNo, po.PPProductionOrdrDate pOrdDate, phase.PPPhaseCfgName phaseName
from PPProductionordrs po inner join PPPhaseCfgs phase on phase.PPPhaseCfgID = po.FK_PPPhaseCfgID where po.AAStatus = 'Alive' AND {0} ORDER BY po.PPProductionOrdrID DESC", sqlCondition);

            return _context.ProductBasicInfoList.FromSqlRaw(sqlBuilding).ToList<ProductBasicInfoList>();
        }

        public List<ProductBasicInfoList> searchData(string sql)
        {
            return _context.ProductBasicInfoList.FromSqlRaw(sql).ToList<ProductBasicInfoList>();
        }
    }
}
