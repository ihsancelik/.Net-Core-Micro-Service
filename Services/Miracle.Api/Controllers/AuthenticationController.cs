using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Authentication;
using Miracle.Api.Responses.Authentication;
using Miracle.Api.Services;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Authentication), ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost, Route(ApiCoreRoutes.AuthenticateRoutes.Authenticate)]
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationModel model)
        {
            if (ModelState.IsValid)
            {
                return await authenticationService.AuthenticateAsync(model);
            }

            var response = new AuthenticationResponse(false);
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route(ApiCoreRoutes.AuthenticateRoutes.AuthenticateByRefreshToken)]
        public async Task<AuthenticationResponse> AuthenticateByRefreshTokenAsync(RefreshTokenModel model)
        {
            if (ModelState.IsValid)
            {
                return await authenticationService.AuthenticateByRefreshTokenAsync(model.WebRefreshToken);
            }

            var response = new AuthenticationResponse(false);
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpGet, MiracleAuthorize, Route(ApiCoreRoutes.AuthenticateRoutes.RevokeAuthenticate)]
        public async Task<AuthenticationResponse> RevokeAuthenticateAsync()
        {
            var token = this.GetToken();
            return await authenticationService.RevokeAuthenticateAsync(token);
        }
    }
}
