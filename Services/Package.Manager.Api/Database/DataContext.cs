using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Package.Manager.Api.Database
{
    public class DataContext : DbContext
    {
        public DbSet<Package> Packages { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }

    public class Package
    {
        [Key]
        public int Id { get; set; }
        public string Platform { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string FilePath { get; set; }
        public string Hash { get; set; }
    }
}