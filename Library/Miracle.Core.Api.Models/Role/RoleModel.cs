using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Role
{
    public class RoleModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Value { get; set; }
    }
}
