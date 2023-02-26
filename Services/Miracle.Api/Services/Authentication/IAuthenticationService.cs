using Miracle.Api.Models.Authentication;
using Miracle.Api.Responses.Authentication;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationModel model);
        public Task<AuthenticationResponse> AuthenticateByRefreshTokenAsync(string refreshToken);
        public Task<AuthenticationResponse> RevokeAuthenticateAsync(string authToken);
    }
}