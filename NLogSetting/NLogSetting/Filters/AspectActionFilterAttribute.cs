using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Linq;
using NLogSetting.Models;

namespace NLogSetting.Filters
{
    public class AspectActionFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private Logger logger;
        public AspectActionFilterAttribute()
        {
             logger = LogManager.GetCurrentClassLogger();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller= (ControllerActionDescriptor)context.ActionDescriptor;
            if (!context.ModelState.IsValid)
            {
                logger.Warn("参数错误：" + JsonConvert.SerializeObject(context.ActionArguments));
                context.Result = new OkObjectResult(null);
            }
            else
            {
                logger.Trace("RquestBody:" + JsonConvert.SerializeObject(context.ActionArguments));
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult)
            {
                OkObjectResult okResult = (OkObjectResult)context.Result;
                ResultData result = new ResultData();
                
                if (context.ModelState.IsValid)
                {
                    result.ToSuccess(okResult.Value);
                    context.Result = new OkObjectResult(result);
                }
                else
                {
                    var values = context.ModelState.Values.Where(i => i.Errors.Count > 0);
                    IList<string> list = new List<string>();
                    foreach (var v in values)
                    {
                        foreach (var err in v.Errors)
                        {
                            list.Add(err.ErrorMessage);
                        }
                    }
                    result.ToErrValidation(string.Join(",", list));
                    context.Result = new OkObjectResult(result);
                }
            }
        }
    }
}