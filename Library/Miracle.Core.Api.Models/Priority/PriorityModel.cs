using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Priority
{
    public class PriorityModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string State { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }
    }
}