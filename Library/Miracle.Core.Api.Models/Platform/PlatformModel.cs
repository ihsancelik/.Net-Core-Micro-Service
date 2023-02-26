using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Platform
{
    public class PlatformModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }
    }
}