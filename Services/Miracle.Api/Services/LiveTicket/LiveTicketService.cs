using Microsoft.EntityFrameworkCore;
using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Responses.Ticket;
using Miracle.Api.Services.Helpers;
using System;
using System.Linq;

namespace Miracle.Api.Services
{
    public class LiveTicketService : ILiveTicketService
    {
        private readonly MainContext db;
        private readonly IBaseRepository<MainContext, LiveChat> liveRepository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public LiveTicketService(MainContext db, IBaseRepository<MainContext, LiveChat> liveRepository,
            IMessageGeneratorService messageGeneratorService)
        {
            this.db = db;
            this.liveRepository = liveRepository;
            this.messageGeneratorService = messageGeneratorService;
        }


        #region Common
        public PagedListResponse<LiveChat> GetPagedListResponse(PaginationParameterModel model)
        {
            throw new System.NotImplementedException();
        }

        public LiveChat Get(int id)
        {
            return liveRepository.Get().FirstOrDefault(s => s.Id == id);
        }

        public GetResponse<LiveChat> GetResponse(int id)
        {
            var data = liveRepository.Get().FirstOrDefault(s => s.Id == id);
            var response = new GetResponse<LiveChat>();
            response.SetData(data);

            return response;
        }

        public IQueryable<LiveChat> GetList()
        {
            return liveRepository.Table;
        }

        public ListResponse<LiveChat> GetListResponse()
        {
            var list = liveRepository.Get().ToList();
            var response = new ListResponse<LiveChat>();
            response.SetData(list);

            return response;
        }

        public CreateResponse CreateResponse(LiveChat value)
        {
            throw new System.NotImplementedException();
        }

        public EmptyResponse UpdateResponse(LiveChat value)
        {
            liveRepository.Table.Update(value);
            var dbResult = liveRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = liveRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("live support", Enums.MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }
            liveRepository.Table.Remove(data);


            var dbResult = liveRepository.Save();
            var response = new EmptyResponse(dbResult);

            LiveChatContent content = new LiveChatContent();

            /*var ImageName = content.ImageName;
            if (response.Success)
                imageManagerService.DeleteTicketImage(ImageName);*/

            return response;
        }

        public GetResponse<object> GetCountResponse()
        {

            var response = new GetResponse<object>();
            response.SetData(liveRepository.Table.Count());
            return response;
        }
        #endregion

        public LiveChat GetRoomId(int id)
        {
            return liveRepository.Get().FirstOrDefault(s => s.Id == id);
        }
        public LiveChat GetRoomName(string roomName)
        {
            return liveRepository.Get().Include(s => s.LiveChatContents).FirstOrDefault(s => s.RoomName == roomName);
        }
        public SendResponse SendMessage(LiveChat value)
        {
            value.CreatedDate = DateTime.Now;

            if (value.Id == 0)
            {
                liveRepository.Create(value);
                var dbResult = liveRepository.Save();
                var response = new SendResponse(dbResult);
                response.SetData(value.Id);
                return response;
            }
            else
            {
                liveRepository.Update(value);
                var dbResult = liveRepository.Save();
                return new SendResponse(dbResult);
            }
        }
        public ListResponse<LiveChat> GetChats(string roomName)
        {
            var chatList = liveRepository.Table
                .Where(s => s.RoomName == roomName)
                .Include(s => s.LiveChatContents)
                .ToList();

            var response = new ListResponse<LiveChat>();
            response.SetData(chatList);
            return response;
        }

        public ListResponse<LiveChatContent> GetContents(string roomName)
        {
            var chatList = liveRepository.Table
                .Where(s => s.RoomName == roomName)
                .Include(s => s.LiveChatContents)
                .Select(s => s.LiveChatContents)
                .FirstOrDefault();

            var response = new ListResponse<LiveChatContent>();
            response.SetData(chatList);
            return response;
        }
        public GetResponse<string> GetCustomerName(string roomName)
        {

            var customerName = liveRepository.Table
                .Where(s => s.RoomName == roomName)
                .Select(s => s.CustomerName)
                .FirstOrDefault();

            //var fro = liveRepository.Table
            //    .Where(s => s.RoomName == roomName)
            //    .Include(s => s.LiveChatContents)
            //    .FirstOrDefault().LiveChatContents
            //    .FirstOrDefault().AdminName;


            var response = new GetResponse<string>();
            response.SetData(customerName);
            return response;
        }
    }
}