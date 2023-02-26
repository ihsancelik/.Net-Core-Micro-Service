using Library.Helpers.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class VersionInfo
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Version { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public ICollection<ProductSetting> ProductSettings { get; }
        [JsonIgnore, IgnoreDataMember]
        public ICollection<User_Version> User_Versions { get; }

        public VersionInfo()
        {
            ProductSettings = new Collection<ProductSetting>();
            User_Versions = new Collection<User_Version>();
        }
    }
}
