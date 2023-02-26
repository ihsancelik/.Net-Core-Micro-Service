using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public string Name { get; set; }

        [MiracleRequired]
        public int Order { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        [MiracleRequired]
        public string ImageName { get; set; }

    }
}