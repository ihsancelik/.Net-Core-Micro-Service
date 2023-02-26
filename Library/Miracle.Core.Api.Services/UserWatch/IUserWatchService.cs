using Library.Responses.Common;
using Miracle.Core.Api.Models.UserWatch;

namespace Miracle.Core.Api.Services.UserWatch
{
    public interface IUserWatchService
    {
        public void SetOnline(int userId);
        public void SetOffline(int userId);
        public void CheckOnlineStates();
        public ListResponse<UserWatchModel> GetOnlineUsers();
        public GetResponse<UserWatchModel> GetOnlineUser(int userId);
        public GetResponse<object> GetCountResponse();
    }
}