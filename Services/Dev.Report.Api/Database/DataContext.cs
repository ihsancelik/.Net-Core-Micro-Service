using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Report.Api.Database
{
    public class DataContext : DbContext
    {
        public DbSet<AppLog> AppLogs { get; set; }
        public DataContext()
        {

        }
    }

    public class AppLog
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Exception { get; set; }
        public string Description { get; set; }
    }
}
