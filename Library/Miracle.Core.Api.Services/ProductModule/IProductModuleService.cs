using Library.Responses.Common;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;

namespace Miracle.Core.Api.Services
{
    public interface IProductModuleService : IBaseResponseService<ProductModule>
    {
        public PagedListResponse<ProductModule> GetListByProductResponse(int productId, PaginationParameterModel paginationModel);
    }
}
