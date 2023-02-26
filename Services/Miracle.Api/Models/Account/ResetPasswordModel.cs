using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Account
{
    public class ResetPasswordModel
    {
        [MiracleRequired, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match")]
        public string Repassword { get; set; }
    }
}