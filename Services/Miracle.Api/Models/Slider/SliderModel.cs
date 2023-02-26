using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Slider
{
    public class SliderModel
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public string Name { get; set; }

        [MiracleRequired]
        public int Order { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public IFormFile SliderImage { get; set; }
    }
}
