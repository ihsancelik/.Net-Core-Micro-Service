using Library.Responses.Common;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;

namespace Miracle.Core.Api.Services
{
    public interface IUserService : IBaseResponseService<User>
    {
        public PagedListResponse<Product> GetProducts(int userId, PaginationParameterModel model);
        public ListResponse<VersionInfo> GetProductVersions(int userId, int productId);
        public PagedListResponse<User_Product_Module> GetProductModules(int userId, int productId, PaginationParameterModel model);
        public GetResponse<ProductLimitation> GetProductLimitation(int userId, int productId);
        public EmptyResponse AddProduct(int userId, int productId, ProductLimitation productLimitation);
        public EmptyResponse RemoveProduct(int userId, int productId);
        public EmptyResponse AddVersion(int userId, int productId, int[] versionInfoIdList);
        public EmptyResponse AddModule(int userId, int productId, int moduleId, bool isActive);
        public EmptyResponse RemoveModule(int userId, int productId, int moduleId);
    }
}