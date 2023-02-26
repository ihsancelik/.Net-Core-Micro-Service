using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(256)]
        public string Address { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MiracleRequired]
        public string Location { get; set; }
    }
}