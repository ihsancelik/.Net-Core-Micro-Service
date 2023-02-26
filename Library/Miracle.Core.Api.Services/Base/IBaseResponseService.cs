using Library.Responses.Common;
using Miracle.Core.Api.Models.Pagination;

namespace Miracle.Core.Api.Services
{
    public interface IBaseResponseService<T> : IBaseService<T> where T : class
    {
        public PagedListResponse<T> GetPagedListResponse(PaginationParameterModel paginationModel);
        public ListResponse<T> GetListResponse();
        public GetResponse<T> GetResponse(int id);
        public EmptyResponse CreateResponse(T model);
        public EmptyResponse UpdateResponse(T model);
        public EmptyResponse DeleteResponse(int id);
        public GetResponse<object> GetCountResponse();
    }
}
