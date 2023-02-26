using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Account
{
    public class RegisterModel
    {

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

        [MiracleRequired, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match")]
        public string Repassword { get; set; }
    }
}
