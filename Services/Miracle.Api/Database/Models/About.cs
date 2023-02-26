using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Title { get; set; }

        [MiracleRequired, MaxLength(1024)]
        public string Text { get; set; }

        [MiracleRequired]
        public string ImageName { get; set; }
    }
}