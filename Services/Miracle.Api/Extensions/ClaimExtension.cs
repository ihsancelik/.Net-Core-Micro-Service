using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Miracle.Api.Extensions
{
    public static class ClaimExtension
    {
        /// <summary>
        /// Userın ID değerini döner. Herhangi bir hata durumunda -1 değeri döner.
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static int GetId(this ControllerBase controllerBase)
        {
            var claim = controllerBase.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                return -1;

            int.TryParse(claim.Value, out int id);
            return id;
        }

        public static string GetToken(this ControllerBase controllerBase)
        {
            var authToken = controllerBase.HttpContext.Request.Headers["Authorization"].ToString();
            return authToken;
        }

        public static IEnumerable<string> GetRoles(this ControllerBase controllerBase)
        {
            var roles = controllerBase.User.Claims
                .Where(s => s.Type == ClaimTypes.Role)
                .Select(s => s.Value);

            return roles;
        }


        public static IEnumerable<string> GetRoles<T>(this Hub<T> hub) where T : class
        {
            var roles = hub.Context.User.Claims
                .Where(s => s.Type == ClaimTypes.Role)
                .Select(s => s.Value);
            return roles;
        }
        public static string GetUserName<T>(this Hub<T> hub) where T : class
        {
            var claim = hub.Context.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Name);
            if (claim == null)
                return string.Empty;

            return claim.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// Userın kullanıcı adını döner. Herhangi bir hata durumunda boş string değeri döner.
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static string GetUsername(this ControllerBase controllerBase)
        {
            var claim = controllerBase.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Name);
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }


        /// <summary>
        /// Userın kullanıcı adını döner. Herhangi bir hata durumunda boş string değeri döner.
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static string GetUsername(HttpContext httpContext)
        {
            if (httpContext == null)
                return string.Empty;

            var claim = httpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Name);
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }
    }
}