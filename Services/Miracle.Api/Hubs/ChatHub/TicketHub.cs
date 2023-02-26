using Microsoft.AspNetCore.SignalR;
using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class TicketHub : Hub<ITicketHub>
    {
        static List<LiveChatModel> OnlineUser = new List<LiveChatModel>();
        private readonly MainContext db;
        private readonly ILiveTicketService liveService;

        public TicketHub(MainContext db, ILiveTicketService liveService)
        {
            this.db = db;
            this.liveService = liveService;
        }

        public override Task OnConnectedAsync()
        {
            var roles = this.GetRoles();
            var isAdmin = roles.Contains("Admin");


            if (!(roles.Contains("Admin") || roles.Contains("SD")))
            {
                Console.WriteLine($"Role count {OnlineUser.Count()}");
                OnlineUser.Add(new LiveChatModel()
                {
                    IsAdmin = isAdmin
                });
            }
            Console.WriteLine($"Hub Connected -> { Context.ConnectionId }");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Hub Disconnected -> { Context.ConnectionId }");
            return base.OnDisconnectedAsync(exception);
        }
        public Task LeaveRoom(string roomName)
        {
            var room = db.LiveChats.Find(roomName);
            if (room != null)
            {
                room.IsConnected = false;
                db.SaveChanges();
            }


            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task GetOnlineUsers()
        {
            await Clients.All.ReceiveOnlineUsers(OnlineUser);
        }
        public async Task GetConnectionId()
        {
            var roles = this.GetRoles();
            var isAdmin = roles.Contains("Admin");

            if (isAdmin)
                await Clients.All.ReceiveConnectionId(Context.ConnectionId);
        }


        //1- User, chat açtı
        public async Task JoinRoomUser(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group("admin").OnListenRoom(roomName); //Admine room bilgisi gitsin
        }

        // 2- admin, adminler odasına girdi
        public async Task JoinRoomAdmin()
        {
            var username = this.GetUserName();
            await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            await Clients.Group("admin").ReceiveAdminInfo(username);

        }

        //3- Mmesaj Gönder
        public async Task SendMessageToRoom(LiveChatModel model)
        {
            var roles = this.GetRoles();
            var admin = roles.Contains("Admin");
            model.IsAdmin = admin;

            var name = this.GetUserName();
            if (name == "") model.AdminName = "admin";
            else model.AdminName = name;

            var connectionId = Context.ConnectionId;

            await Clients.Group("admin").OnListenMessageToAdmin(model.CustomerName, model.IsAdmin, model.Message, model.RoomName, model.Company);
            await Clients.Group(model.RoomName).ReceiveMessage(model.CustomerName, model.AdminName, model.IsAdmin, model.Message);
            await Clients.Group(model.AdminName).ReceiveAdminInfoToUser(model.AdminName);

            #region Create Chat
            var room = db.LiveChats.Where(s => s.RoomName == model.RoomName).FirstOrDefault();
            if (room == null)
            {
                LiveChat liveChat = new LiveChat()
                {
                    RoomName = model.RoomName,
                    CustomerName = model.CustomerName,
                    Company = model.Company,
                    ConnectionId = connectionId,
                    IsConnected = false,
                };

                var response = liveService.SendMessage(liveChat);

                if (model.IsAdmin == true)
                {
                    var chatContent = new LiveChatContent()
                    {
                        IsAdmin = model.IsAdmin,
                        AdminName = model.AdminName,
                        Message = model.Message,
                        CreatedDate = DateTime.Now,
                        LiveChatId = response.Id
                    };
                    var chat = liveService.GetRoomId(response.Id);
                    chat.Id = response.Id;
                    chat.LiveChatContents.Add(chatContent);

                    liveService.SendMessage(chat);
                    Console.WriteLine($"Admin write -> { model.RoomName }");

                }
                else
                {
                    var chatContent = new LiveChatContent()
                    {
                        IsAdmin = model.IsAdmin,
                        AdminName = name,
                        Message = model.Message,
                        CreatedDate = DateTime.Now,
                        LiveChatId = response.Id
                    };
                    var chat = liveService.GetRoomId(response.Id);
                    chat.Id = response.Id;
                    chat.LiveChatContents.Add(chatContent);

                    liveService.SendMessage(chat);
                }
            }
            else
            {
                var liveChat = liveService.GetRoomName(model.RoomName);

                Console.WriteLine($"User write -> { model.RoomName }");
                var messageContent = new LiveChatContent()
                {
                    IsAdmin = model.IsAdmin,
                    AdminName = model.AdminName,
                    Message = model.Message,
                    CreatedDate = DateTime.Now,
                    LiveChatId = liveChat.Id
                };

                liveChat.LiveChatContents.Add(messageContent);
                liveService.SendMessage(liveChat);
            }
            #endregion
        }

        //Admin, user chate geçti
        public async Task JoinToUserRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        //Click
        public async Task ClickRoom(bool click, string roomName)
        {
            await Clients.Groups(roomName).ReceiveClickRoom(click, roomName);
        }

        //Notification(
        public async Task SendNotification(string roomName, bool notification)
        {
            await Clients.Groups("admin").NotificationToAdmin(roomName, notification);
            await Clients.Groups(roomName).NotificationToUser(notification);
        }
    }
}