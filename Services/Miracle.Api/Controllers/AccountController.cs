using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Account;
using Miracle.Api.Responses.Account;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Account), ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost, AllowAnonymous, Route(ApiCoreRoutes.AccountRoutes.Register)]
        public async Task<RegisterResponse> RegisterAsync([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                return await accountService.RegisterResponseAsync(model);
            }

            return new RegisterResponse(this.GetModelStateErrors());
        }

        [HttpPost, AllowAnonymous, Route(ApiCoreRoutes.AccountRoutes.ForgotPassword)]
        public async Task<EmptyResponse> ForgotPasswordRequest([FromBody] ForgotPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                return await accountService.ForgotPasswordRequestAsync(model);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPost, AllowAnonymous, Route(ApiCoreRoutes.AccountRoutes.ChangePassword)]
        public async Task<EmptyResponse> ForgotPasswordResponseAsync([FromBody] ForgotPasswordResponseModel model)
        {
            if (ModelState.IsValid)
            {
                return await accountService.ForgotPasswordResponseAsync(model);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize, Route(ApiCoreRoutes.AccountRoutes.ResetPasswordOutSource)]
        public async Task<EmptyResponse> ResetPasswordOutSource([FromBody] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var authToken = this.GetToken();
                return await accountService.ResetPasswordOutSource(model, authToken);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }
    }
}