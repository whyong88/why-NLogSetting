using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using NLog;

namespace NLogSetting.Middlewares
{
    public class RequestMonitorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger logger;

        public RequestMonitorMiddleware(RequestDelegate next)
        {
            _next = next;
            logger = LogManager.GetCurrentClassLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.Log(NLog.LogLevel.Trace, "\r\nRequestStart:--------------------------------------------------------------");
            StringBuilder urlSb = new StringBuilder();
            urlSb.Append(context.Request.Scheme);
            urlSb.Append("://");
            urlSb.Append(context.Request.Host);
            urlSb.Append(context.Request.Path);
            urlSb.Append($" | Method:{context.Request.Method}");
            logger.Log(NLog.LogLevel.Trace, $"RequestUrl:{urlSb}");


            IList<string> _headers = new List<string>();
            foreach (var h in context.Request.Headers)
            {
                _headers.Add($"{h.Key}:{h.Value}");
            }

            if (_headers.Any())
            {
                logger.Log(NLog.LogLevel.Trace, $"RequestHeaer:{{{string.Join(",", _headers)}}}");
            }

            await _next(context);
        }
    }

    public static class RequestMonitorMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMonitor(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestMonitorMiddleware>();
        }
    }

}