using Miracle.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface ITicketHub
    {
        //JoinToAdmin
        Task ReceiveAdminInfo(string username);

        //JoinRoomUser
        Task OnListenRoom(string roomName);

        //GetOnlineUsers 
        Task ReceiveOnlineUsers(List<LiveChatModel> clients);

        //GetConnectionId
        Task ReceiveConnectionId(string connectionId);

        //SendMessageToRoom
        Task ReceiveMessage(string username, string adminName, bool isAdmin, string message);
        Task OnListenMessageToAdmin(string username, bool isAdmin, string message, string roomName, string company);
        Task ReceiveAdminInfoToUser(string name);

      //  Task PushNotificationInfo(bool notification, int count);

        //Click
        Task ReceiveClickRoom(bool click, string roomName);

        //Notification
        Task NotificationToAdmin(string roomName, bool notification);
        Task NotificationToUser(bool notification);
    }
}