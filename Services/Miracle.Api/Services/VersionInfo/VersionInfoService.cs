using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class VersionInfoService : IVersionInfoService
    {
        private readonly HTTPManagerService httpManagerService;

        public VersionInfoService(HTTPManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService;
        }
        public async Task<GetResponse<string>> GetVersionById(int id, string authToken)
        {
            var response = await httpManagerService.GetAsync<GetResponse<string>>($"versionInfo/getByIdOutSource/{id}",authToken);
            return response != null ? response : new GetResponse<string>();
        }
        public async Task<ListResponse<object>> GetListAll(string authToken)
        {
            var response = await httpManagerService.GetAsync<ListResponse<object>>("versionInfo/GetlistOutSource",authToken);
            return response != null ? response : new ListResponse<object>();
        }
    }
}
