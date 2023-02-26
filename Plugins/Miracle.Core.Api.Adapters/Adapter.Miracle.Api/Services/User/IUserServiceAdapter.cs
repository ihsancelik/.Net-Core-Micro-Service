using Library.Responses.Common;

namespace Adapter.Miracle.Api.Services
{
    public interface IUserServiceAdapter
    {
        public EmptyResponse AddProductOutSource(int userId, string tag, int versionId);
    }
}
