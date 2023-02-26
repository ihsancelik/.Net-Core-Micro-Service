using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Extensions;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.VersionInfo), ApiController]
    public class VersionInfoController : ControllerBase
    {
        private readonly IVersionInfoService versionInfoService;

        public VersionInfoController(IVersionInfoService versionInfoService)
        {
            this.versionInfoService = versionInfoService;
        }

        [HttpGet, Route(CRUDRoutes.GetById)]
        public async Task<GetResponse<string>> GetVersionById([FromRoute]int id)
        {
            var authToken = this.GetToken();
            return await versionInfoService.GetVersionById(id, authToken);
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public async Task<ListResponse<object>> GetListAll()
        {
            var authToken = this.GetToken();
            return await versionInfoService.GetListAll(authToken);
        }
    }
}