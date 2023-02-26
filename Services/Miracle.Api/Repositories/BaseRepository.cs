using Microsoft.EntityFrameworkCore;
using Miracle.Api.Database;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Miracle.Api.Repositories
{
    public class BaseRepository<Context, T> : IBaseRepository<Context, T>
        where Context : BaseContext
        where T : class
    {
        private readonly Context db;
        public DbSet<T> Table { get; }

        public BaseRepository(Context db)
        {
            this.db = db;
            Table = db.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return Table;
        }
        public T Get(Expression<Func<T, bool>> expression)
        {
            return Table.FirstOrDefault(expression);
        }
        public T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions)
        {
            var query = Table.Where(expression);
            return expressions.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).FirstOrDefault();
        }


        public void Create(T entity)
        {
            Table.Add(entity);
        }
        public void Delete(T entity)
        {
            Table.Remove(entity);
        }
        public void Update(T entity)
        {
            Table.Update(entity);
        }
        public DBResult Save()
        {
            var dbResult = db.Save();
            return dbResult;
        }
        public async Task<DBResult> SaveAsync()
        {
            var dbResult = await db.SaveAsync();
            return dbResult;
        }
    }
}
