using gmc_api.DTO.CommonData;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;
using gmc_api.Base.dto;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("favor")]
    public class FavoriestsController : ControllerBase
    {
        private readonly IFavoriestService _favorrService;

        public FavoriestsController(IFavoriestService favorrService)//, IProductionOrderService producservice)
        {
            _favorrService = favorrService;
            //  _producservice = producservice;
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpPost]
        public IActionResult Create(FavoriestCreateRequest favor)
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            favor.FK_ADUserID = userInfo.UserID;
            var checkContain = _favorrService.checkExistModule(favor.ADUserShortCutModule, favor.FK_ADUserID);
            if(checkContain > 0)
            {
                return BadRequest(new { message = "Module added to ShortCut!!!" });
            }
            var fav = _favorrService.CreateObject(favor, userInfo);
            if (fav == null)
                return BadRequest(new { message = "Data input not correct" });
            return Ok(fav);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetFavoritesList()
        {
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var fav = _favorrService.GetAll(string.Format(@"FK_ADUserID = {0}", userInfo.UserID));
            return Ok(fav);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var updateCount = _favorrService.DeleteObject(id);
            if (updateCount == 0)
                return BadRequest(new { message = "No record updated, please check again input data" });
            return Ok(id);
        }
    }
}
