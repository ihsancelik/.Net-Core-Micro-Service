using Library.Helpers.Constraints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Miracle.Core.Api.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Security
{
    public static class TokenValidator
    {
        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
        public static Task OnTokenValidated(TokenValidatedContext context)
        {
            try
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<MainContext>();
                var user = db.Users.FirstOrDefault(s => s.Username == context.Principal.Identity.Name);

                if (user == null)
                {
                    context.Fail("Invalid Token");
                    return Task.CompletedTask;
                }

                var authType = context.Principal.Claims.FirstOrDefault(s => s.Type == "Authentication-Type")?.Value;
                if (string.IsNullOrEmpty(authType))
                {
                    context.Fail("Invalid Token");
                    return Task.CompletedTask;
                }

                var requestToken = context.HttpContext.Request.Headers["Authorization"].ToString().Substring(7);
                if (string.IsNullOrEmpty(requestToken))
                {
                    context.Fail("Invalid Token");
                    return Task.CompletedTask;
                }

                if (authType == AuthTypeConstraints.Application)
                {
                    if (user.Token == null)
                        context.Fail("Invalid Token");

                    if (user.Token != requestToken)
                        context.Fail("Invalid Token");
                }
                else
                {
                    if (user.WebToken == null)
                        context.Fail("Invalid Token");

                    if (user.WebToken != requestToken)
                        context.Fail("Invalid Token");
                }
            }
            catch
            {
                context.Fail("Invalid Token");
            }

            return Task.CompletedTask;
        }
    }
}
