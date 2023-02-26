using Auth.Api.Services;
using Library.Helpers.Extensions;
using Library.Helpers.Security;
using Library.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Controllers
{
    [ApiController, Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost, Route("login")]
        public GetResponseObject Login([FromBody] LoginModel model)
        {
            var response = new GetResponseObject();
            if (ModelState.IsValid)
            {
                var tokenType = this.GetTokenType();
                var userAgent = this.GetUserAgent();
                var password = SHA512Encryptor.Encrypt(model.Password);
                var result = authService.Login(model.Username, password, tokenType, userAgent);

                if (result)
                    response.SetData(new
                    {
                        Token = authService.GeneratedToken,
                        RefreshToken = authService.GeneratedRefreshToken
                    });
                else
                    response.AddErrorList(authService.Exception);

                return response;
            }

            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpGet, Route("get-token-types")]
        public GetResponseObject GetTokenTypes()
        {
            var response = new GetResponseObject();
            response.SetData(authService.GetTokenTypes());
            return response;
        }
    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}