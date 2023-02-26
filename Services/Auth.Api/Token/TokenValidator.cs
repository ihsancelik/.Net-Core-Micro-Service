using Auth.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Api.Token
{
    public static class TokenValidator
    {
        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.Now;
            }
            return false;
        }



        public static Task OnTokenValidated(TokenValidatedContext context)
        {
            try
            {
                int.TryParse(context.Principal.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value, out int userId);
                var authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();
                var token = context.GetRequestToken();
                var userIsValid = authService.UserIsValid(userId, token);

                if (!userIsValid)
                {
                    context.Fail(authService.Exception);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                context.Fail(ex);
            }

            return Task.CompletedTask;
        }
        public static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.CompletedTask;
        }

        private static string GetRequestToken(this TokenValidatedContext context)
        {
            try
            {
                var requestToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(requestToken))
                    requestToken = context.HttpContext.Request.Query["access_token"].ToString();
                else
                    requestToken = requestToken.Substring(7);

                return requestToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TokenValidator Exception: {ex.Message}");
                return "";
            }
        }
    }
}
