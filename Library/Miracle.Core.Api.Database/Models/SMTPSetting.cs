using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class SMTPSetting
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Host { get; set; }

        [MiracleRequired]
        public int Port { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired, DataType(DataType.Password)]
        public string Password { get; set; }

        [MiracleRequired]
        public bool EnableSSL { get; set; }
    }
}