using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System.ComponentModel.DataAnnotations;
using NLogSetting.Models;

namespace NLogSetting.Filters
{
    public class AspectExceptionFilter : IExceptionFilter
    {
        private Logger logger;
        public AspectExceptionFilter()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void OnException(ExceptionContext context)
        {
            var logger = LogManager.GetCurrentClassLogger();
            ResultData result = new ResultData();

            result.ToErrValidation(context.Exception.Message);
            if (context.Exception is FriendlyValidationException)
            {
                logger.Warn(context.Exception.Message);
            }
            else
            {
                logger.Error(context.Exception);
            }

            context.Result = new OkObjectResult(result);
            context.ExceptionHandled = true;
        }
    }

    public class FriendlyValidationException : ValidationException
    {
        public FriendlyValidationException(string message) : base(message)
        {

        }
    }
}