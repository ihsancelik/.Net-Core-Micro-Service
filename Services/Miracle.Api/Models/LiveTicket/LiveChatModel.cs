namespace Miracle.Api.Models
{
    public class LiveChatModel
    {
        public string CustomerName { get; set; }
        public string AdminName { get; set; }
        public string Message { get; set; }
        public string Company { get; set; }
        public string RoomName { get; set; }
        public bool IsAdmin { get; set; }
    }
}