using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Services;
using static Library.Routes.ApiCoreRoutes;
namespace Adapter.Miracle.Api.Controllers
{
    [Route(ControllerRoutes.VersionInfo), ApiController]
    public class VersionInfoController : ControllerBase
    {
        private readonly IVersionInfoService versionInfoService;

        public VersionInfoController(IVersionInfoService versionInfoService)
        {
            this.versionInfoService = versionInfoService;
        }

        [HttpGet, Route(VersionInfoRoutes.GetByIdOutSource)]
        public GetResponse<string> GetByIdOutSource([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var version = versionInfoService.Get(id)?.Version;
                var response = new GetResponse<string>();
                response.SetData(version);
                return response;
            }
            return new GetResponse<string>(this.GetModelStateErrors());
        }

        [HttpGet, Route(VersionInfoRoutes.GetListOutSource)]
        public ListResponse<object> GetListOutSource()
        {
            if (ModelState.IsValid)
            {
                var list = versionInfoService.GetList();
                var response = new ListResponse<object>();
                response.SetData(list);
                return response;
            }
            return new ListResponse<object>(this.GetModelStateErrors());
        }
    }
}