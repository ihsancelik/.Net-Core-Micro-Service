using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Plugin
{
    public class PluginModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Tag { get; set; }
        [MiracleRequired, MaxLength(64)]
        public string Version { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Type { get; set; }

        [MiracleRequired]
        public int ProductId { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public IFormCollection FormData { get; set; }
    }
}