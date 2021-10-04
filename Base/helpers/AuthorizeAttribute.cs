using gmc_api.Base.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace gmc_api.Base.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string Roles;

        public AuthorizeAttribute(string roles)
        {
            Roles = roles;
        }

        public AuthorizeAttribute()
        {
            Roles = "";
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserLoginInfo)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

    }
}
