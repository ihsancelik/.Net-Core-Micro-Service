using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Account
{
    public class ForgotPasswordResponseModel
    {
        [MiracleRequired, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match")]
        public string Repassword { get; set; }

        [MiracleRequired]
        public string Code { get; set; }
    }
}
