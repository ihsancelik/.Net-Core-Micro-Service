using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class ContactForm
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string FullName { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MiracleRequired, MaxLength(256)]
        public string Message { get; set; }
    }
}
