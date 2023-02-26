using Library.Responses.Common;
using Library.Responses.Database;
using Miracle.Core.Api.Database.Models;

namespace Miracle.Core.Api.Services
{
    public interface IAccountService
    {
        public EmptyResponse ResetPassword(int id, string password);

        public EmptyResponse ForgotPasswordRequest(string email);
        public EmptyResponse ForgotPasswordResponse(string code, string password);

        public DatabaseResponse Create(User model);
        public EmptyResponse CreateResponse(User model);
    }
}