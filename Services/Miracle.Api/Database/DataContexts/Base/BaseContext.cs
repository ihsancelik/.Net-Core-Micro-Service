using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Miracle.Api.Database
{
    public class BaseContext : DbContext, IBaseContext
    {
        public BaseContext()
        {
        }

        public DBResult Save()
        {
            DBResult response;
            try
            {
                int result = SaveChanges();
                response = new DBResult(true, result);
            }
            catch (Exception ex)
            {
                response = new DBResult(ex);
            }

            return response;
        }
        public async Task<DBResult> SaveAsync()
        {
            Task<DBResult> response;
            try
            {
                int result = await SaveChangesAsync();
                response = Task.FromResult(new DBResult(true, result));
            }
            catch (Exception ex)
            {
                response = Task.FromResult(new DBResult(ex));
            }

            return await response;
        }
    }
}