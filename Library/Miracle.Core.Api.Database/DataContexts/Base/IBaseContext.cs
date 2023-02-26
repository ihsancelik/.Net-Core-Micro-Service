using Library.Responses.Database;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Database
{
    interface IBaseContext
    {
        public DatabaseResponse Save();
        public Task<DatabaseResponse> SaveAsync();
    }
}
