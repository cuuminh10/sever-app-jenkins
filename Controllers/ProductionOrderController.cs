using gmc_api.DTO.PP;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static gmc_api.Base.Helpers.Constants;
using gmc_api.Base.dto;

namespace gmc_api.Controllers
{

    [ApiController()]
    [Route("productOrder")]
    public class ProductionOrderController : ControllerBase
    {
        private readonly IProductionOrderService _service;
        private readonly IPPProductionOrdrEstFGService _serviceFG;
        private readonly IADAttachmentService _attach;
        private readonly IADCommentService _comment;
        private readonly ISystemCommonService _common;

        public ProductionOrderController(IProductionOrderService service, IPPProductionOrdrEstFGService serviceFG, IADAttachmentService attach, IADCommentService comment, ISystemCommonService common)
        {
            _service = service;
            _attach = attach;
            _comment = comment;
            _common = common;
            _serviceFG = serviceFG;
        }

        [Authorize]
        [HttpPost("{type}/{statusType}")]
        public IActionResult GetAll(string type, string statusType)
        {
            if (type == ProductType.JOB_TIKCET)
            {
                if (!ProductType.isIn(type) || !ProductOrderStatus.isIn(statusType))
                {
                    return BadRequest(new { message = "Data input not correct!!!" });
                }
            }
            var users = _service.GetAll(type, statusType);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("groups/{type}")]
        public IActionResult GroupData(string type)
        {
            if (!ProductType.isIn(type))
            {
                return BadRequest(new { message = "Data input not correct!!!" });
            }
            var users = _service.GetCountByGroupAll(type);
            return Ok(users);
        }


        [Authorize]
        [HttpPost("search/v2/{type}")]
        public IActionResult searchData(string type, [FromBody] ProductionOrderSearch data)
        {
            if (!ProductType.isIn(type))
            {
                return BadRequest(new { message = "Data input not correct!!!" });
            }
            if(data.groupByColumn != DEFAULT_VALUE_STRING
                && data.groupByColumn.Trim().Length != 0)
            {
                Utils.SetPropertyValue(data, data.groupByColumn, data.groupByValue);
            } else if (data.groupByValue != DEFAULT_VALUE_STRING)
            {
                data.groupByColumn = "status";
                Utils.SetPropertyValue(data, data.groupByColumn, data.groupByValue);
            }
            var users = _service.searchData(type, data);
            return Ok(users);
        }

        [Authorize]
        [HttpPost("groups/v2/{type}")]
        public IActionResult GroupData(string type, [FromBody] ProductionOrderSearch data)
        {
            if (!ProductType.isIn(type))
            {
                return BadRequest(new { message = "Data input not correct!!!" });
            }
            var users = _service.GetCountData(type, data);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("detail/v2/{type}/{no}")]
        public IActionResult GetObjectByNoV2(string type, string no)
        {
            if (!ProductType.isIn(type))
            {
                return BadRequest(new { message = ProductType.messageValidate() });
            }
            if (type == ProductType.JOB_TIKCET)
            {
                var detail = _service.GetDetailJobTicketByNo(Uri.UnescapeDataString(no));
                if (detail == null)
                    return BadRequest(new { message = "Data input not correct!!!" });
                var items = _service.GetListJobticketItems(detail.id);
                var conv = _comment.GetConvData(type, detail.id);
                var rs = _service.CombineJobTicketResult(detail, items, conv);
                return Ok(rs);
            }
            else
            {
                var detail = _service.GetDetailProdRstByNo(Uri.UnescapeDataString(no));
                if (detail == null)
                    return BadRequest(new { message = "Data input not correct!!!" });
                var items = _service.GetListProdRstItems(detail.id);
                var conv = _comment.GetConvData(type, detail.id);
                var rs = _service.CombineProdRstResult(detail, items, conv);
                return Ok(rs);
            }
        }

        [Authorize]
        [HttpGet("detail/{type}/{no}")]
        public IActionResult GetObjectByNo(string type, string no)
        {
            if (!ProductType.isIn(type))
            {
                return BadRequest(new { message = ProductType.messageValidate() });
            }
            if (type == ProductType.JOB_TIKCET)
            {
                var detail = _service.GetDetailJobTicketByNo(Uri.UnescapeDataString(no));
                if (detail == null)
                    return BadRequest(new { message = "Data input not correct!!!" });
                var items = _service.GetListJobticketItems(detail.id);
                var attact = _attach.GetAll(string.Format(@" ADAttachmentTable = '{0}' AND ADAttachmentRefID = '{1}'", FileUploadType.UploadTable[type], detail.id));
                var comment = _comment.GetAll(string.Format(@" ADCommentTable = '{0}' AND ADCommentRefID = '{1}'", FileUploadType.UploadTable[type], detail.id));
                var rs = _service.CombineJobTicketResult(detail, items, attact, _service.CovertComment(comment.ToList()));
                return Ok(rs);
            }
            else
            {
                var detail = _service.GetDetailProdRstByNo(Uri.UnescapeDataString(no));
                if (detail == null)
                    return BadRequest(new { message = "Data input not correct!!!" });
                var items = _service.GetListProdRstItems(detail.id);
                var attact = _attach.GetAll(string.Format(@" ADAttachmentTable = '{0}' AND ADAttachmentRefID = '{1}'", FileUploadType.UploadTable[type], detail.id));
                var comment = _comment.GetAll(string.Format(@" ADCommentTable = '{0}' AND ADCommentRefID = '{1}'", FileUploadType.UploadTable[type], detail.id));
                var rs = _service.CombineProdRstResult(detail, items, attact, _service.CovertComment(comment.ToList()));
                return Ok(rs);
            }
        }
        
        ///
        [Authorize]
        [HttpGet("scanProdRstFromJT/{no}")]
        public IActionResult scanProdRstFromJobticket(string no)
        {
            // Kiểm tra trạng thái
            var detail = _service.GetDetailJobTicketByNo(Uri.UnescapeDataString(no));
            if (detail == null)
                return BadRequest(new { message = "Data input not correct!!!" });
            var items = _service.GetListJobticketItemsKQSX(detail.id);
            if (items == null || items.Count == 0)
            {
                return BadRequest(new { message = "Phiếu không có chi tiết hoặc đã ra hết số lượng!" });// Phiếu không có chi tiết hoặc đã ra hết số lượng!
            }
            if (items.Any(u => u.closed > 0))
            {
                return BadRequest(new { message = "Phiếu đã đóng!" });
            }
            var itemCheckLastPhase = _serviceFG.GetAllObjectEntityMapper(string.Format(" FK_PPProductionOrdrID = {0} AND PPProductionOrdrEstFGRQty > 0 ", detail.id));
            var itemFinall = new List<JobTicketItemsKQSXInfo>();
            foreach (var jbEstFG in itemCheckLastPhase)
            {
                var objNextPhase = _common.CheckHaveNextObjectByRoutingIDAndPhase(jbEstFG.FK_PPRoutingID, jbEstFG.FK_PPPhaseCfgID);
                var lstItemLayout = new List<PPMSLayoutItemsInfo>();
                if (objNextPhase.counts == 0)
                {
                    lstItemLayout = _common.GetListLayoutItemRevisionByWO(jbEstFG.FK_ICProductRootID, jbEstFG.FK_ICProductID, jbEstFG.FK_PPWOID);
                    lstItemLayout.ForEach(objLayout =>
                    {
                        var fg = _common.getItemsKQSXByLayout(objLayout.PPMSLayoutItemID);
                        fg.qty = jbEstFG.PPProductionOrdrEstFGRQty * objLayout.PPMSLayoutItemQty;
                        itemFinall.Add(fg);
                    });
                }
                if (lstItemLayout.Count == 0)
                {
                    itemFinall.Add(items.Where(u => u.itemID == jbEstFG.PPProductionOrdrEstFGID).First());
                }
            }
            ProdRstDetail rs = _service.CreateTmpProdRst(detail, itemFinall);
            rs.no = "**NEW**";
            rs.jobTicketNo = no;
            rs.id = -1;
            return Ok(rs);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult updateProdRst(int id, ProdRstUpdateRequest data)
        {
            if (data.description != DEFAULT_VALUE_STRING)
            {
                _service.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrID", id },
                    { "PPProductionOrdrDesc", data.description}
                });
            }
            var completedCount = 0;
            int elementCount = 0;
            if (data.detail != null && data.detail.Count > 0)
            {
                data.detail.ForEach(item =>
                {
                    elementCount++;
                    // call update process for jobticket
                    // tính toán lại cho KQSX và update
                    var oldProdData = _serviceFG.GetObjectEntityMapper(item.id);
                    if (oldProdData.PPProductionOrdrEstFGLayout == 0) // Not layout data
                    {
                        oldProdData.PPProductionOrdrEstFGQty = (item.qty - item.cancelQty - item.NCRQty);
                        oldProdData.PPProductionOrdrEstFGNCRQty = item.NCRQty;
                        oldProdData.PPProductionOrdrEstFGCQty = item.cancelQty;
                        oldProdData.PPProductionOrdrEstFGSetupQty = item.setUpQty;

                        // TODO FK_PPProductionOrdrID
                        oldProdData.PPProductionOrdrEstFGQCQty = item.qty;
                        oldProdData.PPProductionOrdrEstFGRQty = item.qty - item.cancelQty;
                        oldProdData.PPProductionOrdrEstFGStkQty = oldProdData.PPProductionOrdrEstFGQty;
                        var oldJobTicketItemData = _serviceFG.GetObjectEntityMapper(oldProdData.FK_PPProductionOrdrEstFGID);
                        oldJobTicketItemData.PPProductionOrdrEstFGRQty = oldJobTicketItemData.PPProductionOrdrEstFGRQty + oldProdData.PPProductionOrdrEstFGOrgQty
                        - item.qty;
                        oldProdData.PPProductionOrdrEstFGOrgQty = item.qty;
                        _common.SetValueAfterValidateUOM(oldProdData.FK_ICUOMID,
                            (PPProductionOrdrEstFGCreate)Utils.CopyObject(oldProdData, new PPProductionOrdrEstFGCreate()), "PPProductionOrdrEstFGs");
                        if (oldJobTicketItemData.PPProductionOrdrEstFGRQty <= 0)
                        {
                            oldJobTicketItemData.PPProductionOrdrEstFGRQty = 0;
                            completedCount++;
                        }
                        // Saving oldJobTicketItemData
                        _serviceFG.UpdateObject(new SortedDictionary<string, object>()
                        {
                            { "PPProductionOrdrEstFGID", oldJobTicketItemData.PPProductionOrdrEstFGID },
                            { "PPProductionOrdrEstFGRQty", oldJobTicketItemData.PPProductionOrdrEstFGRQty }
                        });
                        // Saving oldProdData
                        _serviceFG.UpdateObject(new SortedDictionary<string, object>()
                        {
                            { "PPProductionOrdrEstFGID", oldProdData.PPProductionOrdrEstFGID },
                            { "PPProductionOrdrEstFGQty", oldProdData.PPProductionOrdrEstFGQty },
                            { "PPProductionOrdrEstFGNCRQty", oldProdData.PPProductionOrdrEstFGNCRQty },
                            { "PPProductionOrdrEstFGCQty", oldProdData.PPProductionOrdrEstFGCQty },
                            { "PPProductionOrdrEstFGSetupQty", oldProdData.PPProductionOrdrEstFGSetupQty },
                            { "PPProductionOrdrEstFGQCQty", oldProdData.PPProductionOrdrEstFGQCQty },
                            { "PPProductionOrdrEstFGRQty", oldProdData.PPProductionOrdrEstFGRQty },
                            { "PPProductionOrdrEstFGStkQty", oldProdData.PPProductionOrdrEstFGStkQty },
                            { "PPProductionOrdrEstFGOrgQty", oldProdData.PPProductionOrdrEstFGOrgQty }
                        });
                    }
                    else
                    {
                        var oldJobTicketItemData = _serviceFG.GetObjectEntityMapper(oldProdData.FK_PPProductionOrdrEstFGID);
                        var objNextPhase = _common.CheckHaveNextObjectByRoutingIDAndPhase(oldJobTicketItemData.FK_PPRoutingID, oldJobTicketItemData.FK_PPPhaseCfgID);
                        var lstItemLayout = new List<PPMSLayoutItemsInfo>();
                        if (objNextPhase.counts == 0)
                        {
                            lstItemLayout = _common.GetListLayoutItemRevisionByWO(oldJobTicketItemData.FK_ICProductRootID, oldJobTicketItemData.FK_ICProductID, oldJobTicketItemData.FK_PPWOID);
                            var numberFGQty = 0M;
                            var numberOldFGQty = 0M;
                            var numberLayoutQty = 0M;
                            lstItemLayout.ForEach(objLayout =>
                            {
                                //oldProdData
                                //var requestItem = data.detail.Where(u => u.id == objLayout.PPMSLayoutItemID).FirstOrDefault();
                                numberFGQty += item.qty;
                                numberOldFGQty += oldProdData.PPProductionOrdrEstFGOrgQty;
                                numberLayoutQty += objLayout.PPMSLayoutItemQty;
                                // Số lượng đạt
                                oldProdData.PPProductionOrdrEstFGQty = (item.qty - item.cancelQty - item.NCRQty);// * objLayout.PPMSLayoutItemQty;
                                oldProdData.PPProductionOrdrEstFGNCRQty = item.NCRQty;// * objLayout.PPMSLayoutItemQty;
                                oldProdData.PPProductionOrdrEstFGCQty = item.cancelQty;// * objLayout.PPMSLayoutItemQty;
                                oldProdData.PPProductionOrdrEstFGSetupQty = item.setUpQty;// * objLayout.PPMSLayoutItemQty;

                                // oldProdData.PPProductionOrdrEstFGLayout = lstItemLayout.Sum(x => x.PPMSLayoutItemQty);

                                // TODO FK_PPProductionOrdrID
                                oldProdData.PPProductionOrdrEstFGQCQty = item.qty;// * objLayout.PPMSLayoutItemQty;
                                oldProdData.PPProductionOrdrEstFGRQty = (item.qty - item.cancelQty);// * objLayout.PPMSLayoutItemQty;
                                oldProdData.PPProductionOrdrEstFGStkQty = oldProdData.PPProductionOrdrEstFGQty;
                                // if(prodFG.PPProductionOrdrEstFGOrgQty == 0)
                                oldProdData.PPProductionOrdrEstFGOrgQty = item.qty;// * objLayout.PPMSLayoutItemQty;

                                _common.SetValuesAfterValidateProductQty(oldProdData.PPProductionOrdrEstFGQty,
                            (PPProductionOrdrEstFGCreate)Utils.CopyObject(oldProdData, new PPProductionOrdrEstFGCreate()), "PPProductionOrdrEstFGs");
                                oldProdData.PPProductionOrdrEstFGLayoutQty = oldProdData.PPProductionOrdrEstFGLayout == 0 ? 0 :
                        Math.Round((oldProdData.FK_ICLayoutProductID > 0 ? oldProdData.PPProductionOrdrEstFGOrgQty : (oldProdData.PPProductionOrdrEstFGOrgQty - oldProdData.PPProductionOrdrEstFGCQty)) / (oldProdData.PPProductionOrdrEstFGLayout), 6);
                                // Saving oldProdData
                                _serviceFG.UpdateObject(new SortedDictionary<string, object>()
                                    {
                                        { "PPProductionOrdrEstFGID", oldProdData.PPProductionOrdrEstFGID },
                                        { "PPProductionOrdrEstFGQty", oldProdData.PPProductionOrdrEstFGQty },
                                        { "PPProductionOrdrEstFGNCRQty", oldProdData.PPProductionOrdrEstFGNCRQty },
                                        { "PPProductionOrdrEstFGCQty", oldProdData.PPProductionOrdrEstFGCQty },
                                        { "PPProductionOrdrEstFGSetupQty", oldProdData.PPProductionOrdrEstFGSetupQty },
                                        { "PPProductionOrdrEstFGQCQty", oldProdData.PPProductionOrdrEstFGQCQty },
                                        { "PPProductionOrdrEstFGRQty", oldProdData.PPProductionOrdrEstFGRQty },
                                        { "PPProductionOrdrEstFGStkQty", oldProdData.PPProductionOrdrEstFGStkQty },
                                        { "PPProductionOrdrEstFGOrgQty", oldProdData.PPProductionOrdrEstFGOrgQty },
                                        { "PPProductionOrdrEstFGLayout", oldProdData.PPProductionOrdrEstFGLayout },
                                        { "PPProductionOrdrEstFGLayoutQty", oldProdData.PPProductionOrdrEstFGLayoutQty }
                                    });
                            });

                            if (lstItemLayout.Count > 0)
                            {
                                oldJobTicketItemData.PPProductionOrdrEstFGRQty = oldProdData.PPProductionOrdrEstFGRQty - ((numberFGQty - numberOldFGQty) / numberLayoutQty);
                                if (oldJobTicketItemData.PPProductionOrdrEstFGRQty <= 0)
                                {
                                    oldJobTicketItemData.PPProductionOrdrEstFGRQty = 0;
                                    completedCount++;
                                }
                                _serviceFG.UpdateObject(new SortedDictionary<string, object>()
                                {
                                    { "PPProductionOrdrEstFGID", oldJobTicketItemData.PPProductionOrdrEstFGID },
                                    { "PPProductionOrdrEstFGRQty", oldJobTicketItemData.PPProductionOrdrEstFGRQty }
                                });
                            }
                        }
                    }
                });
            }
            var rootJobticket = _service.GetObjectEntityMapper(id);
            if (completedCount == elementCount)
            {
                _service.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrID", rootJobticket.PPProductionOrdrID },
                    { "PPProductionOrdrStatus", "Transfered" }
                });
            }
            else
            {
                _service.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrID", rootJobticket.PPProductionOrdrID },
                    { "PPProductionOrdrStatus", "Transfering" }
                });
            }
            return Ok(new { id = id });
        }
        [Authorize]
        [HttpPost("createProdRst/{no}")]
        public IActionResult createProdRst(string no, [FromBody] ProdRstCreateRequest data)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var detail = _service.GetDetailJobTicketByNo(Uri.UnescapeDataString(no));
            if (detail == null)
                return BadRequest(new { message = "Data input not correct!!!" });
            var rootJobticket = _service.GetObjectEntityMapper(detail.id);
            var items = _serviceFG.GetAllObjectEntityMapper(string.Format(" FK_PPProductionOrdrID = {0} ", detail.id));
            var prodRst = (PPProductionOrdrCreate)Utils.CopyObject(rootJobticket, new PPProductionOrdrCreate());
            prodRst.PPProductionOrdrDate = DateTime.Now;
            prodRst.PPProductionOrdrTypeCombo = ProductType.DBValue[ProductType.P_RESULT];
            prodRst.FK_GLTranCfgID = _common.GetDefaultTranCfgIDByOrgTranCfgID(userInfo.UserID, prodRst.PPProductionOrdrTypeCombo, 0);
            prodRst.PPProductionOrdrNo = _common.GetGeneralNoItem(prodRst, TableModule.ProducResult);
            prodRst.FK_PPProductionOrdrParentID = detail.id;

            var lstProdFGItems = new List<PPProductionOrdrEstFGCreate>();
            int completedCount = 0;
            int elementCount = 0;
            foreach (var jbEstFG in items)
            {
                elementCount++;
                var numberFGQty = 0M;
                var numberLayoutQty = 0M;
                var objNextPhase = _common.CheckHaveNextObjectByRoutingIDAndPhase(jbEstFG.FK_PPRoutingID, jbEstFG.FK_PPPhaseCfgID);
                var lstItemLayout = new List<PPMSLayoutItemsInfo>();
                if (objNextPhase.counts == 0)
                {
                    lstItemLayout = _common.GetListLayoutItemRevisionByWO(jbEstFG.FK_ICProductRootID, jbEstFG.FK_ICProductID, jbEstFG.FK_PPWOID);
                    lstItemLayout.ForEach(objLayout =>
                    {

                        var prodFG = new PPProductionOrdrEstFGCreate();
                        Utils.CopyObject(jbEstFG, prodFG);
                        Utils.CopyObject(objLayout, prodFG);
                        var requestItem = data.detail.Where(u => u.itemID == objLayout.PPMSLayoutItemID).First();
                        numberFGQty += requestItem.qty;
                        numberLayoutQty += objLayout.PPMSLayoutItemQty;
                        // Số lượng đạt
                        prodFG.PPProductionOrdrEstFGQty = (requestItem.qty - requestItem.cancelQty - requestItem.NCRQty);// * objLayout.PPMSLayoutItemQty;
                        prodFG.PPProductionOrdrEstFGNCRQty = requestItem.NCRQty;// * objLayout.PPMSLayoutItemQty;
                        prodFG.PPProductionOrdrEstFGCQty = requestItem.cancelQty;// * objLayout.PPMSLayoutItemQty;
                        prodFG.PPProductionOrdrEstFGSetupQty = requestItem.setUpQty;// * objLayout.PPMSLayoutItemQty;

                        prodFG.PPProductionOrdrEstFGLayout = lstItemLayout.Sum(x => x.PPMSLayoutItemQty);

                        prodFG.FK_PPProductionOrdrEstFGID = jbEstFG.PPProductionOrdrEstFGID;
                        // TODO FK_PPProductionOrdrID
                        prodFG.PPProductionOrdrEstFGQCQty = requestItem.qty;// * objLayout.PPMSLayoutItemQty;
                        prodFG.PPProductionOrdrEstFGRQty = (requestItem.qty - requestItem.cancelQty);// * objLayout.PPMSLayoutItemQty;
                        prodFG.PPProductionOrdrEstFGStkQty = prodFG.PPProductionOrdrEstFGQty;
                        // if(prodFG.PPProductionOrdrEstFGOrgQty == 0)
                        prodFG.PPProductionOrdrEstFGOrgQty = requestItem.qty;// * objLayout.PPMSLayoutItemQty;
                        //var FK_PPProductionOrdrEstFGID = 0;
                        prodFG.PPProductionOrdrEstFGQCStatusCombo = "New";
                        prodFG.FK_PPProductionOrdrID = 0;// FK TỚI KQSX CHA
                        //Số lượng gốc, không được sửa
                        prodFG.FK_ICLayoutProductID = jbEstFG.FK_ICProductID;
                        _common.SetValuesAfterValidateProductQty(prodFG.PPProductionOrdrEstFGQty, prodFG, "PPProductionOrdrEstFGs");
                        prodFG.PPProductionOrdrEstFGLayoutQty = prodFG.PPProductionOrdrEstFGLayout == 0 ? 0 :
                Math.Round((prodFG.FK_ICLayoutProductID > 0 ? prodFG.PPProductionOrdrEstFGOrgQty : (prodFG.PPProductionOrdrEstFGOrgQty - prodFG.PPProductionOrdrEstFGCQty)) / (prodFG.PPProductionOrdrEstFGLayout), 6);
                        lstProdFGItems.Add(prodFG);
                    });

                    if (lstItemLayout.Count > 0)
                    {
                        jbEstFG.PPProductionOrdrEstFGRQty = jbEstFG.PPProductionOrdrEstFGRQty - (numberFGQty / numberLayoutQty);
                        if (jbEstFG.PPProductionOrdrEstFGRQty <= 0)
                        {
                            jbEstFG.PPProductionOrdrEstFGRQty = 0;
                            completedCount++;
                        }
                    }
                }
                if (lstItemLayout.Count == 0)
                {
                    var prodFG = new PPProductionOrdrEstFGCreate();
                    var requestItem = data.detail.Where(u => u.itemID == jbEstFG.PPProductionOrdrEstFGID).First();
                    Utils.CopyObject(jbEstFG, prodFG);
                    prodFG.PPProductionOrdrEstFGQty = (requestItem.qty - requestItem.cancelQty - requestItem.NCRQty);
                    prodFG.PPProductionOrdrEstFGNCRQty = requestItem.NCRQty;
                    prodFG.PPProductionOrdrEstFGCQty = requestItem.cancelQty;
                    prodFG.PPProductionOrdrEstFGSetupQty = requestItem.setUpQty;

                    prodFG.FK_PPProductionOrdrEstFGID = jbEstFG.PPProductionOrdrEstFGID;
                    // TODO FK_PPProductionOrdrID
                    prodFG.PPProductionOrdrEstFGQCQty = requestItem.qty;
                    prodFG.PPProductionOrdrEstFGRQty = requestItem.qty - requestItem.cancelQty;
                    prodFG.PPProductionOrdrEstFGStkQty = prodFG.PPProductionOrdrEstFGQty;
                    prodFG.PPProductionOrdrEstFGOrgQty = requestItem.qty;
                    //var FK_PPProductionOrdrEstFGID = 0;
                    prodFG.PPProductionOrdrEstFGQCStatusCombo = "New";
                    prodFG.FK_PPProductionOrdrID = 0;// FK TỚI KQSX CHA

                    jbEstFG.PPProductionOrdrEstFGRQty = jbEstFG.PPProductionOrdrEstFGRQty - requestItem.qty;

                    _common.SetValueAfterValidateUOM(prodFG.FK_ICUOMID, prodFG, "PPProductionOrdrEstFGs");
                    if (jbEstFG.PPProductionOrdrEstFGRQty <= 0)
                    {
                        jbEstFG.PPProductionOrdrEstFGRQty = 0;
                        completedCount++;
                    }
                    lstProdFGItems.Add(prodFG);
                }
            }
            // Save main jobticket
            if (completedCount == elementCount)
            {
                _service.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrID", rootJobticket.PPProductionOrdrID },
                    { "PPProductionOrdrStatus", "Transfered" }
                });
            }
            else
            {
                _service.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrID", rootJobticket.PPProductionOrdrID },
                    { "PPProductionOrdrStatus", "Transfering" }
                });
            }
            // Create main kqsx
            prodRst.PPProductionOrdrDesc = data.description;
            //prodRst.PPProductionOrdrDate = data.ordDate;
            var podCreate = _service.CreateObject(prodRst, userInfo);
            // Update item jobticket
            foreach (var jbEstFG in items)
            {
                _serviceFG.UpdateObject(new SortedDictionary<string, object>()
                {
                    { "PPProductionOrdrEstFGID", jbEstFG.PPProductionOrdrEstFGID },
                    { "PPProductionOrdrEstFGRQty", jbEstFG.PPProductionOrdrEstFGRQty }
                });
            }
            // Create item KQSX
            foreach (var prodFGItems in lstProdFGItems)
            {
                prodFGItems.FK_PPProductionOrdrID = podCreate.PPProductionOrdrID;
                _serviceFG.CreateObjectNotReponse(prodFGItems, userInfo);
            }
            return Ok(new { id = podCreate.PPProductionOrdrID });
        }
    }
}
