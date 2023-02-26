using Adapter.Miracle.Api.Services;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Adapter.Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Company), ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServiceAdapter companyServiceAdapter;
        public CompanyController(ICompanyServiceAdapter companyServiceAdapter)
        {
            this.companyServiceAdapter = companyServiceAdapter;
        }

        [HttpGet, MiracleAuthorize(Roles.Admin), Route("ticket")]
        public ListResponse<object> GetUserForTicket()
        {
            if (ModelState.IsValid)
                return companyServiceAdapter.GetUserForTicket();

            return new ListResponse<object>(this.GetModelStateErrors());
        }
    }
}