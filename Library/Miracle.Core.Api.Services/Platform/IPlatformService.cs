using Library.Responses.Common;
using Miracle.Core.Api.Database.Models;

namespace Miracle.Core.Api.Services
{
    public interface IPlatformService : IBaseResponseService<Platform>
    {
        public ListResponse<Platform> GetListByProductId(int productId);
    }
}