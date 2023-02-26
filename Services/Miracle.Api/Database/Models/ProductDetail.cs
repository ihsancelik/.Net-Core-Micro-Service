using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Api.Database.Models
{
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(50)]
        public string Title { get; set; }

        [MiracleRequired, MaxLength(256)]
        public string Content { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Product Product { get; set; }
    }
}
