using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;

namespace Miracle.Api.Services
{
    public interface IAboutService : IBaseService<About>
    {
        public About GetFirst();
        public GetResponse<string> GetAboutImagePath();
    }
}