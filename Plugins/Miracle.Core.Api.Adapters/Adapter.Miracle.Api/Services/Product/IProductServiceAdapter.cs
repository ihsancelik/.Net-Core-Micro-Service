using Library.Responses.Common;

namespace Adapter.Miracle.Api.Services
{
    public interface IProductServiceAdapter
    {
        public ListResponse<object> GetProducts();
    }
}
