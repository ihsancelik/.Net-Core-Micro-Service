using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Account;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Account), ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly DataHelper dataHelper;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
            dataHelper = new DataHelper();
        }

        [HttpPost,AllowAnonymous, Route(AccountRoutes.Register)]
        public EmptyResponse Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                user.ImageName = "avatar.png";

                var result = dataHelper.FieldBinder(model, user);
                return result ? accountService.CreateResponse(user) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        #region ResetPassword

        [HttpPost, MiracleAuthorize(), Route(AccountRoutes.ResetPassword)]
        public EmptyResponse ResetPassword([FromRoute] int id, [FromBody] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                return accountService.ResetPassword(id, model.Password);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }


        [HttpPost, MiracleAuthorize(), Route(AccountRoutes.ResetPasswordOutSource)]
        public EmptyResponse ResetPasswordOutSource([FromBody] ResetPasswordModel model)
        {
            var id = this.GetId();

            if (ModelState.IsValid)
            {
                return accountService.ResetPassword(id, model.Password);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }
        #endregion

        #region ForgotPassword

        [HttpPost, Route(AccountRoutes.ForgotPassword)]
        public EmptyResponse ForgotPasswordRequest([FromBody] ForgotPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                return accountService.ForgotPasswordRequest(model.Email);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPost, Route(AccountRoutes.ChangePassword)]
        public EmptyResponse ForgotPasswordResponse([FromBody] ForgotPasswordResponseModel model)
        {
            if (ModelState.IsValid)
            {
                return accountService.ForgotPasswordResponse(model.Code, model.Password);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        #endregion
    }
}