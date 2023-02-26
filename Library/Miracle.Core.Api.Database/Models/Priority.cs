using Library.Helpers.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    [JsonObject(IsReference = true)]
    public class Priority
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string State { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; }
        public ICollection<ProductSetting> ProductSettings { get; }

        public Priority()
        {
            Users = new Collection<User>();
            ProductSettings = new Collection<ProductSetting>();
        }
    }
}