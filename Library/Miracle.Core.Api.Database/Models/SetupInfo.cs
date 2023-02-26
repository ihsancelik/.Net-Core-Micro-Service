using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class SetupInfo
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }


        [ForeignKey("Platform")]
        public int PlatformId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Platform Platform { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }

        [ForeignKey("VersionInfo")]
        public int VersionInfoId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public VersionInfo VersionInfo { get; set; }
    }
}
