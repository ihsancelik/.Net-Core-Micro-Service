using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models
{
    public class UpdateModel
    {

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Surname { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Username { get; set; }

        [MiracleRequired]
        public string PhoneNumber { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IFormFile ProfilePhoto { get; set; }
    }
}
