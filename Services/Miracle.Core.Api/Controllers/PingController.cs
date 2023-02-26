using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Services.UserWatch;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Ping)]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IUserWatchService userWatchService;
        public PingController(IUserWatchService userWatchService)
        {
            this.userWatchService = userWatchService;
        }

        [Route(PingRoutes.PingUnAuthorize)]
        [HttpGet]
        public EmptyResponse PingUnAuthorize()
        {
            return new EmptyResponse();
        }

        [Route(PingRoutes.PingAuthorize)]
        [HttpGet, MiracleAuthorize]
        public EmptyResponse Test()
        {
            return new EmptyResponse();
        }

        [Route(PingRoutes.PingOnline)]
        [HttpGet, MiracleAuthorize]
        public EmptyResponse PingOnline()
        {
            var userId = this.GetId();

            if (userId > 0)
                userWatchService.SetOnline(userId);

            return new EmptyResponse();
        }
        [Route(PingRoutes.PingOffline)]
        [HttpGet, MiracleAuthorize]
        public EmptyResponse PingOffline()
        {
            var userId = this.GetId();

            if (userId > 0)
                userWatchService.SetOffline(userId);

            return new EmptyResponse();
        }
    }
}
