using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class User_Product_Module
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Module")]
        public int ProductModuleId { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public User User { get; set; }
        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }
        public ProductModule Module { get; set; }
    }
}
