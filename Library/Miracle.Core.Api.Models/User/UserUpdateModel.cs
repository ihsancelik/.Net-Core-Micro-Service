using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models
{
    public class UserUpdateModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Surname { get; set; }

        [MiracleRequired]
        public string PhoneNumber { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Username { get; set; }

        public bool IsActive { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public int[] RoleIdList { get; set; }

        public int? CompanyId { get; set; }

        public int? PriorityId { get; set; }

        public int? MachineId { get; set; }

    }
}
