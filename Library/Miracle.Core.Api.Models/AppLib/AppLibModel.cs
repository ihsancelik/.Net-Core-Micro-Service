using Microsoft.AspNetCore.Http;

namespace Miracle.Core.Api.Models.AppLib
{
    public class AppLibModel
    {
        public int Id { get; set; }
        public string LibName { get; set; }
        public bool IsActive { get; set; }
        public IFormFile LibFile { get; set; }
    }
}
