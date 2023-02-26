using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Account
{
    public class ForgotPasswordRequestModel
    {
        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
