using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;
using Miracle.Api.Responses.Ticket;

namespace Miracle.Api.Services
{
    public interface ILiveTicketService : IBaseService<LiveChat>
    {
        public LiveChat GetRoomId(int id);
        public LiveChat GetRoomName(string roomName);
        public SendResponse SendMessage(LiveChat value);
        public ListResponse<LiveChat> GetChats(string roomName);
        public ListResponse<LiveChatContent> GetContents(string roomName);
        
        public GetResponse<string> GetCustomerName(string roomName);
    }
}