using Library.Helpers.Attributes;
using Library.Helpers.Constraints;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Models.Authentication;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Authentication), ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost, Route(AuthenticateRoutes.Authenticate)]
        public AuthenticationResponse Authenticate(AuthenticationModel model)
        {
            if (ModelState.IsValid)
            {
                var username = model.Username.ToLower();
                var password = model.Password;

                var authenticationType = HttpContext.Request.Headers["Authentication-Type"].ToString();

                if (string.IsNullOrEmpty(authenticationType))
                {
                    return new AuthenticationResponse("Auth header missing!");
                }

                return authenticationService.Authenticate(username, password, authenticationType);
            }

            var response = new AuthenticationResponse(false);
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route(AuthenticateRoutes.AuthenticateByRefreshToken)]
        public AuthenticationResponse AuthenticateByRefreshToken(RefreshTokenModel model)
        {
            if (ModelState.IsValid)
            {
                var authenticationType = HttpContext.Request.Headers["Authentication-Type"].ToString();

                if (string.IsNullOrEmpty(authenticationType))
                {
                    return new AuthenticationResponse("Auth header missing!");
                }

                return authenticationService.AuthenticateByRefreshToken(model.RefreshToken, authenticationType);
            }

            var response = new AuthenticationResponse(false);
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpGet, Route(AuthenticateRoutes.RevokeAuthenticate), MiracleAuthorize()]
        public AuthenticationResponse RevokeAuthenticate()
        {
            var id = this.GetId();

            return authenticationService.RevokeAuthenticate(id, AuthTypeConstraints.Application);
        }

        [HttpGet, MiracleAuthorize, Route(AuthenticateRoutes.RevokeAuthenticateByAdmin)]
        public AuthenticationResponse RevokeAuthenticateByAdmin(int id)
        {
            return authenticationService.RevokeAuthenticate(id, AuthTypeConstraints.Application);
        }
    }
}