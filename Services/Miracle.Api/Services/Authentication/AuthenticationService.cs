using Miracle.Api.Models.Authentication;
using Miracle.Api.Responses.Authentication;
using Miracle.Api.Services.Helpers;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    /// <summary>
    /// JWT Bearer Token Authentication yardımıyla kullanıcı yetkilendirmesi görevini üstlenir.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HTTPManagerService httpManagerService;
        public AuthenticationService(HTTPManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationModel model)
        {
            return await httpManagerService.PostAsync<AuthenticationModel, AuthenticationResponse>("authentication/authenticate", model);
        }
        public async Task<AuthenticationResponse> AuthenticateByRefreshTokenAsync(string refreshToken)
        {
            return await httpManagerService.PostAsync<string, AuthenticationResponse>("authentication/authenticatebyrefreshtoken", refreshToken);
        }
        public async Task<AuthenticationResponse> RevokeAuthenticateAsync(string authToken)
        {
            return await httpManagerService.GetAsync<AuthenticationResponse>("authentication/revokeauthenticate", authToken);
        }
    }
}