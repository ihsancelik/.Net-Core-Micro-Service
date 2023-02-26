using Miracle.Api.Models.UserWatch;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Threading.Tasks;

namespace Miracle.Api.Services.UserWatch
{
    public class UserWatchService : IUserWatchService
    {
        private readonly HTTPManagerService httpManagerService;

        public UserWatchService(HTTPManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService;
        }

        public async Task<GetResponse<UserWatchModel>> GetOnlineUserAsync(int userId, string authToken)
        {
            return await httpManagerService.GetAsync<GetResponse<UserWatchModel>>($"userWatch/getOnlineUser/{userId}", authToken);
        }
        public async Task<ListResponse<UserWatchModel>> GetOnlineUsersAsync(string authToken)
        {
            return await httpManagerService.GetAsync<ListResponse<UserWatchModel>>("userWatch/getOnlineUsers", authToken);
        }
    }
}
