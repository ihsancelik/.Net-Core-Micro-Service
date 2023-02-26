using Adapter.Miracle.Api.Services;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using static Library.Routes.ApiCoreRoutes;

namespace Adapter.Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Role), ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServiceAdapter roleServiceAdapter;
        public RoleController(IRoleServiceAdapter roleServiceAdapter)
        {
            this.roleServiceAdapter = roleServiceAdapter;
        }

        [HttpGet, Route(RoleRoutes.GetByUsername)]
        public ListResponse<string> GetByUsername([FromRoute] string username)
        {
            if (ModelState.IsValid)
                return roleServiceAdapter.GetByUsername(username);

            return new ListResponse<string>(this.GetModelStateErrors());
        }
    }
}