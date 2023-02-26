using Microsoft.EntityFrameworkCore;
using Miracle.Api.Database;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Miracle.Api.Repositories
{
    public interface IBaseRepository<Context, T>
        where Context : DbContext
        where T : class
    {
        DbSet<T> Table { get; }

        IQueryable<T> Get();
        T Get(Expression<Func<T, bool>> expression);
        T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        DBResult Save();
        Task<DBResult> SaveAsync();
    }
}
