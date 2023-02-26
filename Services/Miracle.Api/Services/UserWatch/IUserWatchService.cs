using Miracle.Api.Models.UserWatch;
using Miracle.Api.Responses.Common;
using System.Threading.Tasks;

namespace Miracle.Api.Services.UserWatch
{
    public interface IUserWatchService
    {
        public Task<ListResponse<UserWatchModel>> GetOnlineUsersAsync(string authToken);
        public Task<GetResponse<UserWatchModel>> GetOnlineUserAsync(int userId, string authToken);
    }
}
