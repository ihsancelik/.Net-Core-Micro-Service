using Library.Responses.Common;

namespace Adapter.Miracle.Api.Services
{
    public interface IRoleServiceAdapter
    {
        ListResponse<string> GetByUsername(string username);
    }
}
