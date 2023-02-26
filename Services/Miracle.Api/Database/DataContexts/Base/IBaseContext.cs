using System.Threading.Tasks;

namespace Miracle.Api.Database
{
    public interface IBaseContext
    {
        public DBResult Save();
        public Task<DBResult> SaveAsync();
    }
}
