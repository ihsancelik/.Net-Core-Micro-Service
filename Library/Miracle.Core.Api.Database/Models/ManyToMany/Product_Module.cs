using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class Product_Module
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Module")]
        public int ProductModuleId { get; set; }


        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public ProductModule Module { get; set; }
    }
}
