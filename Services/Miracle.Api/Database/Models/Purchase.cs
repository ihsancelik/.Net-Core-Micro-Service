using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        public int VersionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyValue { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }    

    }
}
