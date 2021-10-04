using gmc_api.Base.Exceptions;
using gmc_api.Base.Helpers;
using gmc_api.DTO.User;
using gmc_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace gmc_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        //  private IProductionOrderService _producservice;

        public UsersController(IUserService userService)//, IProductionOrderService producservice)
        {
            _userService = userService;
            //  _producservice = producservice;
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public IActionResult Login(LoginRequest model)
        {
            var response = _userService.Login(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            response.moduleList = new System.Collections.Generic.List<RoleOfUser>();// _userService.roleInSytems(response.ADUserID);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [HttpPost]
        public IActionResult Create(UserCreateRequest user)
        {
            var users = _userService.CreateObject(user);
            if (user == null)
                return BadRequest(new { message = "Data input not correct" });
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Update(int id)
        {
            var users = _userService.GetObject(id);
            return Ok(users);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        //  [ServiceFilter(typeof(ValidateEntityExistsAttribute<User>))]
        //  [TypeFilter(typeof(ValidateEntityExistsAttribute<User>), Arguments = new object[] { "ADUserID" })]
        public IActionResult Update(int id, UserUpdateRequest user)
        {
            user.ADUserID = id;
            var listProp = Utils.getPropertiesForUpdate(user);
            var updateCount = _userService.UpdateObject(listProp);
            if (updateCount == 0)
                return BadRequest(new { message = "No record updated, please check again input data" });
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var updateCount = _userService.DeleteObject(id);
            if (updateCount == 0)
                return BadRequest(new { message = "No record updated, please check again input data" });
            return Ok(id);
        }
    }
}
