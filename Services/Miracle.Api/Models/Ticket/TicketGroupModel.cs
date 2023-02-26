using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models
{
    public class TicketGroupModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public IFormFile ImageName { get; set; }
        public string SelectedProduct { get; set; }
    }
}