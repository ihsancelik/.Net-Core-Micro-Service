using Library.Helpers.Attributes;
using Library.Helpers.Constraints;
using Library.Responses.Core.Api.API;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.StaticDatas;
using System.Linq;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.API), MiracleAuthorize(Roles.SD)]
    public class APIController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;

        public APIController(IAuthenticationService authenticationService, IUserService userService)
        {
            this.authenticationService = authenticationService;
            this.userService = userService;
        }

        [HttpGet, Route(APIRoutes.StopServer)]
        public string StopServer([FromServices] IHostApplicationLifetime hostApplicationLifetime)
        {
            hostApplicationLifetime.StopApplication();
            return "Shutdown!";
        }

        [HttpGet, Route(APIRoutes.GetServerInfo)]
        public GetServerInfoResponse GetServerInfo()
        {
            return new GetServerInfoResponse()
            {
                CurrentDate = StaticDataServerInfo.CurrentDate,
                StartDate = StaticDataServerInfo.StartDate,
                TotalRequestCount = StaticDataServerInfo.TotalRequestCount
            };
        }

        [HttpGet, Route(APIRoutes.LogoutUser)]
        public void LogoutUser(int userId)
        {
            authenticationService.RevokeAuthenticate(userId, AuthTypeConstraints.Application);
        }

        [HttpGet, Route(APIRoutes.LogoutAllUsers)]
        public void LogoutAllUsers()
        {
            var userIdList = userService.GetList().Select(s => s.Id).ToList();

            if (userIdList != null || userIdList.Count > 0)
            {
                int count = userIdList.Count;
                for (int i = 0; i < count; i++)
                {
                    var userId = userIdList[i];
                    authenticationService.RevokeAuthenticate(userId, AuthTypeConstraints.Application);
                }
            }
        }

        [HttpGet, Route(APIRoutes.GetOnlineUsers)]
        public object GetOnlineUsers()
        {
            // Aktif olan kullanıcılar için ileride user tablosuna online bool değeri eklenebilir.

            var onlineUsers = userService.GetList().Where(s => s.Token != null).Select(s =>
              new
              {
                  s.Id,
                  s.Username,
              });

            return onlineUsers;
        }

        [HttpGet, Route(APIRoutes.GetOfflineUsers)]
        public object GetOfflineUsers()
        {
            // Aktif olan kullanıcılar için ileride user tablosuna online bool değeri eklenebilir.

            var offlineUsers = userService.GetList().Where(s => s.Token != null).Select(s =>
              new
              {
                  s.Id,
                  s.Username,
              });

            return offlineUsers;
        }
    }
}
