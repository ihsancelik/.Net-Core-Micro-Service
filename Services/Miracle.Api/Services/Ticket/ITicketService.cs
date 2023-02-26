using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;
using Miracle.Api.Responses.Ticket;

namespace Miracle.Api.Services
{
    public interface ITicketService : IBaseService<TicketGroup>
    {
        public ListResponse<TicketGroup> GetMessageGroups(int userId);
        public ListResponse<TicketMessage> GetTicketMessages(int groupId);
        public GetResponse<string> GetTicketImage(int messageId);
        public SendResponse SendResponse(TicketGroup value);
    }
}