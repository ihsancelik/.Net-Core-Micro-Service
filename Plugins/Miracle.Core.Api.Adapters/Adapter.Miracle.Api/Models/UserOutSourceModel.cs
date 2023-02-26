using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Adapter.Miracle.Api.Models
{
    public class UserOutSourceModel
    {
        public string Guid { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Surname { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Username { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired]
        public string PhoneNumber { get; set; }

        public IFormFile ProfilePhoto { get; set; }
    }
}
