using Library.Responses.Common;
using Miracle.Core.Api.Database.Models;

namespace Miracle.Core.Api.Services
{
    public interface IPriorityService : IBaseResponseService<Priority>
    {
        public GetResponse<Priority> GetResponseByVersion(int productId, int versionInfoId);
    }
}