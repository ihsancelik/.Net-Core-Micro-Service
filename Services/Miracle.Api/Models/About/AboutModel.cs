using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.About
{
    public class AboutModel
    {
        [MiracleRequired, MaxLength(128)]
        public string Title { get; set; }

        [MiracleRequired, MaxLength(1024)]
        public string Text { get; set; }     

        public IFormFile AboutImage { get; set; }
    }
}
