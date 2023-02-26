using Library.Responses.Common;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;

namespace Miracle.Core.Api.Services
{
    public interface IVersionInfoService : IBaseResponseService<VersionInfo>
    {
        public PagedListResponse<VersionInfo> GetListByProductResponse(int productId, PaginationParameterModel paginationModel);
        public ListResponse<VersionInfo> GetListByUserProduct(int productId);
    }
}