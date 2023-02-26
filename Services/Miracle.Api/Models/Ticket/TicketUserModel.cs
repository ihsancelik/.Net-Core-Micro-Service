using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models
{
    public class TicketUserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Company { get; set; }
    }
}