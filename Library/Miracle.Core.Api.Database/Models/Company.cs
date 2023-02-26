using Library.Helpers.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Address { get; set; }

        public string Location { get; set; }

        [MiracleRequired, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ICollection<User> Users { get; }

        public Company()
        {
            Users = new Collection<User>();
        }
    }
}