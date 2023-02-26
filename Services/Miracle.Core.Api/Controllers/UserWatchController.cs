using Library.Helpers.Attributes;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Models.UserWatch;
using Miracle.Core.Api.Services.UserWatch;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.UserWatch)]
    [ApiController]
    public class UserWatchController : ControllerBase
    {
        private readonly IUserWatchService userWatchService;

        public UserWatchController(IUserWatchService userWatchService)
        {
            this.userWatchService = userWatchService;
        }

        [Route(UserWatchRoutes.GetOnlineUsers)]
        [HttpGet, MiracleAuthorize]
        public ListResponse<UserWatchModel> GetOnlineUsers()
        {
            return userWatchService.GetOnlineUsers();
        }


        [Route(UserWatchRoutes.GetOnlineUser)]
        [HttpGet, MiracleAuthorize]
        public GetResponse<UserWatchModel> GetOnlineUser(int userId)
        {
            return userWatchService.GetOnlineUser(userId);
        }


        [HttpGet, MiracleAuthorize, Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return userWatchService.GetCountResponse();
        }
    }
}
