using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Product
{
    public class ProductDetailModel
    {
        [Key]
        public int Id { get; set; }
        [MiracleRequired]
        public string Title { get; set; }
        [MiracleRequired]
        public string Content { get; set; }
    }
}
