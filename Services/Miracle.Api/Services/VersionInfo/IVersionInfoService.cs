using Miracle.Api.Responses.Common;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IVersionInfoService
    {
        public Task<GetResponse<string>> GetVersionById(int id, string authToken);
        public Task<ListResponse<object>> GetListAll(string authToken);
    }
}
