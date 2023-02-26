using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Account
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

        [MiracleRequired, DataType(DataType.Password)]
        public string Repassword { get; set; }


    }
}
