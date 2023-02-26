using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.User), ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet, Route(UserRoutes.GetProfileImageOutSource)]
        public async Task<GetResponse<string>> GetProfileImageAsync()
        {
            var authToken = this.GetToken();
            return await userService.GetUserImageAsync(authToken);
        }

        #region OutSource
        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.GetById)]
        public async Task<GetResponse<User>> GetOutSourceByIdAsync([FromRoute] int id)
        {
            var authToken = this.GetToken();
            return await userService.GetById(id, authToken);
        }

        [HttpGet, MiracleAuthorize, Route(UserRoutes.GetOutSource)]
        public async Task<GetResponse<User>> GetOutSourceAsync()
        {
            var authToken = this.GetToken();
            return await userService.Get(authToken);
        }

        [HttpPut, MiracleAuthorize, Route(UserRoutes.UpdateOutSource)]
        public async Task<EmptyResponse> UpdateOutSourceAsync([FromForm] UserOutSourceModel model)
        {
            var authToken = this.GetToken();

            return await userService.UpdateUserOutSourceAsync(model, authToken);
        }
        #endregion
    }
}