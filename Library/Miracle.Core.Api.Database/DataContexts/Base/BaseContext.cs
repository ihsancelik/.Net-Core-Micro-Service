using Library.Responses.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Database
{
    public abstract class BaseContext : DbContext, IBaseContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {

        }
        public DatabaseResponse Save()
        {
            DatabaseResponse response;
            try
            {
                int result = SaveChanges();
                response = new DatabaseResponse(true, result);
            }
            catch (Exception ex)
            {
                response = new DatabaseResponse(ex);
            }

            return response;
        }
        public async Task<DatabaseResponse> SaveAsync()
        {
            Task<DatabaseResponse> response;
            try
            {
                int result = await SaveChangesAsync();
                response = Task.FromResult(new DatabaseResponse(true, result));
            }
            catch (Exception ex)
            {
                response = Task.FromResult(new DatabaseResponse(ex));
            }

            return await response;
        }
    }
}
