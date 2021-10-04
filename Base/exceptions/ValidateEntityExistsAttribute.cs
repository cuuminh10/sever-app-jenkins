using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace gmc_api.Base.Exceptions
{
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class
    {
        private readonly GMCContext _context;
        private readonly string _columnId = "ADUserId";
        public ValidateEntityExistsAttribute(GMCContext context)
        {
            _context = context;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int idObj;
            if (context.ActionArguments.ContainsKey("id"))
            {
                idObj = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }
            var entity = _context.Set<T>().AsQueryable().Where(s => (int)s.GetType().GetProperty(_columnId).GetValue(s) == idObj).FirstOrDefault();
            if (entity == null)
            {
                context.Result = new NotFoundResult();
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
