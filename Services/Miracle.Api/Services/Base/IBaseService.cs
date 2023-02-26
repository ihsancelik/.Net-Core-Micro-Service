using System.Linq;

namespace Miracle.Api.Services
{
    public interface IBaseService<T> : IBaseResponseService<T> where T : class
    {
        public T Get(int id);
        public IQueryable<T> GetList();
    }
}
