using Miracle.Api.Models.Account;
using Miracle.Api.Responses.Account;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly HTTPManagerService httpManagerService;

        public AccountService(HTTPManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService;
        }
        public async Task<RegisterResponse> RegisterResponseAsync(RegisterModel model)
        {
            return await httpManagerService.PostAsync<RegisterModel, RegisterResponse>("account/register", model);
        }
        public async Task<EmptyResponse> ForgotPasswordRequestAsync(ForgotPasswordRequestModel model)
        {
            return await httpManagerService.PostAsync<ForgotPasswordRequestModel, EmptyResponse>("account/forgotpassword", model, "");
        }
        public async Task<EmptyResponse> ForgotPasswordResponseAsync(ForgotPasswordResponseModel model)
        {
            return await httpManagerService.PostAsync<ForgotPasswordResponseModel, EmptyResponse>("account/changepass", model, "");
        }
        public async Task<EmptyResponse> ResetPasswordOutSource(ResetPasswordModel model, string authToken)
        {
            return await httpManagerService.PostAsync<ResetPasswordModel, EmptyResponse>("account/resetPass", model, authToken);
        }
    }
}