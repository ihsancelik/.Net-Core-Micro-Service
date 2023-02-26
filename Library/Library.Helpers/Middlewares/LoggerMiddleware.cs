using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Helpers.Middlewares
{
    public class LoggerMiddleware
    {
        public string MessageTemplate = ",{RemoteIpAddress},{Username},{RequestMethod},{RequestPath},{StatusCode},{Elapsed:0.0000},{UserAgent}";
        public string LogFolderName = "Logs";
        public string LogFileName = "Log-.txt";
        public RollingInterval RollingInterval = RollingInterval.Day;
        public long? FileSizeLimitBytes = null;
        public int? RetainedFileCountLimit = 31;

        public List<string> IgnoredPaths = new List<string>();

        ILogger Log = Serilog.Log.ForContext<LoggerMiddleware>();
        readonly RequestDelegate _next;
        public LoggerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            InitializeLogConfiguration(env);

            if (next == null)
                throw new ArgumentNullException(nameof(next));

            _next = next;
        }

        private void InitializeLogConfiguration(IWebHostEnvironment env)
        {
            var logFolder = Path.Combine(env.WebRootPath, LogFolderName);
            var logFile = Path.Combine(logFolder, LogFileName);

            Log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFile,
                rollingInterval: RollingInterval,
                fileSizeLimitBytes: FileSizeLimitBytes,
                retainedFileCountLimit: RetainedFileCountLimit)
                .CreateLogger();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var sw = Stopwatch.StartNew();

            try
            {
                await _next(httpContext);
                sw.Stop();

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;

                var method = httpContext.Request.Method;
                var path = httpContext.Request.Path.Value.ToLower();

                if (method == "OPTIONS")
                    return;

                if (path == "/" || IgnoredPaths.Contains(path))
                    return;

                log.Write(level, MessageTemplate,
                    httpContext.Connection?.RemoteIpAddress.ToString(),
                    httpContext.User.Identity.Name,
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    statusCode,
                    sw.Elapsed.TotalMilliseconds,
                    httpContext.Request.Headers["User-Agent"].ToString().Replace(",", ""));
            }
            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (LogException(httpContext, sw, ex)) { }
        }

        bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate,
                httpContext.Connection?.RemoteIpAddress.ToString(),
                httpContext.User.Identity.Name,
                httpContext.Request.Method,
                httpContext.Request.Path,
                500,
                sw.Elapsed.TotalMilliseconds,
                httpContext.Request.Headers["User-Agent"].ToString().Replace(",", ""));

            return false;
        }

        ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }
    }
}
