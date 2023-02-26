using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Library.Helpers.Extensions
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
        public static int GetId(HttpContext httpContext)
        {
            if (httpContext == null)
                return -1;

            var claim = httpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                return -1;

            int.TryParse(claim.Value, out int id);
            return id;
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
