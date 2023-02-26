using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class User_Version
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("VersionInfo")]
        public int VersionInfoId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public User User { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public VersionInfo VersionInfo { get; set; }
    }
}
