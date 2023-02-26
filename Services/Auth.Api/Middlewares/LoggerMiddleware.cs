using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Auth.Api.Middlewares
{
    public class LoggerMiddleware : Library.Helpers.Middlewares.LoggerMiddleware
    {
        public LoggerMiddleware(RequestDelegate next, IWebHostEnvironment env) : base(next, env)
        {
            IgnoredPaths.Add("/test/test");
            IgnoredPaths.Add("/favicon.ico");
            IgnoredPaths.Add("/test/test1");
            IgnoredPaths.Add("/test/test2");
        }
    }
}
