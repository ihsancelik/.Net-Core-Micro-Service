using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Authentication
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
