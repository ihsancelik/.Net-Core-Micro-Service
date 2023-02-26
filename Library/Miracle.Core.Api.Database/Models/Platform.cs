using Library.Helpers.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public ICollection<Platform_Product> PlatformProducts { get; }

        public Platform()
        {
            PlatformProducts = new Collection<Platform_Product>();
        }
    }
}