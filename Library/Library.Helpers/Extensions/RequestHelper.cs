using Microsoft.AspNetCore.Mvc;

namespace Library.Helpers.Extensions
{
    public static class RequestHelper
    {
        public static string GetCustomHeader(this ControllerBase controller, string headerName)
        {
            var value = controller.HttpContext.Request.Headers[headerName].ToString();
            return value;
        }
        public static string GetTokenType(this ControllerBase controller)
        {
            var tokenType = controller.HttpContext.Request.Headers["token_type"].ToString();
            return tokenType;
        }
        public static string GetUserAgent(this ControllerBase controller)
        {
            var userAgent = controller.HttpContext.Request.Headers["User-Agent"].ToString();
            return userAgent;
        }
    }
}
