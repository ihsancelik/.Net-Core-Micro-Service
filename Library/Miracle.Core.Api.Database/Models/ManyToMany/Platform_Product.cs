using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class Platform_Product
    {
        [ForeignKey("Platform")]
        public int PlatformId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Platform Platform { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }
    }
}
