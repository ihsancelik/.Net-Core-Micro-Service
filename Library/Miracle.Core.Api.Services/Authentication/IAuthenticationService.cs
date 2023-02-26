using Library.Responses.Common;

namespace Miracle.Core.Api.Services
{
    public interface IAuthenticationService
    {
        public AuthenticationResponse Authenticate(string username, string password, string authType);
        public AuthenticationResponse AuthenticateByRefreshToken(string refreshToken, string authType);
        public AuthenticationResponse RevokeAuthenticate(int userId, string authType);
    }
}