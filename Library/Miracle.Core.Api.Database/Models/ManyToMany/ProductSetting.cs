using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class ProductSetting
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("VersionInfo")]
        public int VersionInfoId { get; set; }

        [ForeignKey("Priority")]
        public int PriorityId { get; set; }


        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public VersionInfo VersionInfo { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Priority Priority { get; set; }
    }
}
