using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;

namespace Miracle.Api.Services
{
    public interface IBaseResponseService<T> where T : class
    {
        public PagedListResponse<T> GetPagedListResponse(PaginationParameterModel model);
        public ListResponse<T> GetListResponse();
        public GetResponse<T> GetResponse(int id);
        public CreateResponse CreateResponse(T value);
        public EmptyResponse UpdateResponse(T value);
        public EmptyResponse DeleteResponse(int id);
        public GetResponse<object> GetCountResponse();
    }
}
