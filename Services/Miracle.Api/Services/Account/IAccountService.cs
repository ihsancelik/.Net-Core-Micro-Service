using Miracle.Api.Models.Account;
using Miracle.Api.Responses.Account;
using Miracle.Api.Responses.Common;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IAccountService
    {
        public Task<RegisterResponse> RegisterResponseAsync(RegisterModel model);
        public Task<EmptyResponse> ResetPasswordOutSource(ResetPasswordModel model, string authToken);
        public Task<EmptyResponse> ForgotPasswordRequestAsync(ForgotPasswordRequestModel model);
        public Task<EmptyResponse> ForgotPasswordResponseAsync(ForgotPasswordResponseModel model);
    }
}
