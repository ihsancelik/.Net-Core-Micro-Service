using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Database
{
    public class CoreDataContext : Miracle.Core.Api.Database.MainContext
    {
        public CoreDataContext(DbContextOptions<CoreDataContext> options) : base(options)
        {

        }
    }
}
