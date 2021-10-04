using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace gmc_api.Base.Exceptions
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errList = new List<String>();
                var propList = context.ActionDescriptor.Parameters[0].ParameterType;
                JsonPropertyNameAttribute propNameForFE;

                string propNameError = "";
                string errorMessageDefault;

                foreach (var prop in context.ModelState)
                {
                    propNameError = prop.Key;
                    var propValue = prop.Value;

                    if (propValue.Errors.Count == 0)
                    {
                        // errList.Add("Input value not mapping with data type");
                        // break;
                        continue;
                    }

                    errorMessageDefault = propValue.Errors.FirstOrDefault().ErrorMessage;
                    var propObj = propList.GetProperty(propNameError);
                    if (propObj == null)
                    {
                        errList.Add("Input value not mapping with data type");
                        break;
                    }

                    propNameForFE = (JsonPropertyNameAttribute)propObj.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault();

                    var regex = new Regex(Regex.Escape(propNameError));
                    var errorMessageUpdate = regex.Replace(errorMessageDefault, propNameForFE.Name, 1);

                    errList.Add(errorMessageUpdate.First().ToString().ToUpper() + errorMessageUpdate.Substring(1));

                }
                context.Result = new BadRequestObjectResult(errList);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

    }
}
