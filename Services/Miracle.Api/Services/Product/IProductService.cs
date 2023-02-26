using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IProductService : IBaseService<Product>
    {
        public Task<ListResponse<object>> GetProductsOutSource();
        public GetResponse<string> GetProductImagePath(int id);
        public ListResponse<Product> GetProductByTag(string tag);
        public Task<EmptyResponse> AddUserProductAsync(int userId, string authToken, string tag, int versionId);
    }
}
