using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Extensions;
using Miracle.Api.Models.UserWatch;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.UserWatch;
using System.Threading.Tasks;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Api.Controllers
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
        public async Task<ListResponse<UserWatchModel>> GetOnlineUsersAsync()
        {
            var authToken = this.GetToken();
            return await userWatchService.GetOnlineUsersAsync(authToken);
        }
        [Route(UserWatchRoutes.GetOnlineUser)]
        [HttpGet, MiracleAuthorize]
        public async Task<GetResponse<UserWatchModel>> GetOnlineUserAsync(int userId)
        {
            var authToken = this.GetToken();
            return await userWatchService.GetOnlineUserAsync(userId, authToken);
        }
    }
}
