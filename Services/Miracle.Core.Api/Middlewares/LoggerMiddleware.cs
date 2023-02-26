using Library.Routes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Middlewares
{
    public class LoggerMiddleware : Library.Helpers.Middlewares.LoggerMiddleware
    {
        public LoggerMiddleware(RequestDelegate next, IWebHostEnvironment env) : base(next, env)
        {
            IgnoredPaths = new List<string>()
            {
                ("/" + ControllerRoutes.Ping + "/" + PingRoutes.PingAuthorize).ToLower(),
                ("/" + ControllerRoutes.Ping + "/" + PingRoutes.PingUnAuthorize).ToLower(),
                ("/" + ControllerRoutes.Ping + "/" + PingRoutes.PingOnline).ToLower(),
                ("/" + ControllerRoutes.Ping + "/" + PingRoutes.PingOffline).ToLower(),
                ("/" + ControllerRoutes.Logging + "/" + LoggingRoutes.GetApiLogs).ToLower(),
                ("/" + ControllerRoutes.QT + "/" + QTRoutes.GetNotice).ToLower()
            };
        }
    }
}
