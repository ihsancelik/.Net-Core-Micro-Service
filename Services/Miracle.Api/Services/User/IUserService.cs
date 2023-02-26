using Miracle.Api.Database.Models;
using Miracle.Api.Models;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IUserService
    {
        public Task<GetResponse<User>> Get(string authToken);
        public Task<GetResponse<User>> GetById(int userId, string authToken);
        public Task<EmptyResponse> UpdateUserOutSourceAsync(UserOutSourceModel model, string authToken);
        public PagedListResponse<Product> GetProducts(int userId, PaginationParameterModel model);
        public Task<GetResponse<string>> GetUserImageAsync(string authToken);
    }
}