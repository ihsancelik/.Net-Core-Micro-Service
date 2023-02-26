using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class User_ProductInfo
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductUsageInfo")]
        public int ProductUsageInfoId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public User User { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }

        public ProductUsageInfo ProductUsageInfo { get; set; }
    }
}
