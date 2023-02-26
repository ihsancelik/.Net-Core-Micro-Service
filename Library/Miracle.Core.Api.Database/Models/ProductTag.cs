using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class ProductTag
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
    }
}