using Library.Responses.Common;

namespace Adapter.Miracle.Api.Services
{
    public interface ICompanyServiceAdapter
    {
        public ListResponse<object> GetUserForTicket();
    }
}