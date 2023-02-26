using Microsoft.EntityFrameworkCore;
using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Responses.Ticket;
using Miracle.Api.Services.Helpers;
using System;
using System.Linq;

namespace Miracle.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly IBaseRepository<MainContext, TicketGroup> ticketRepository;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;
        private readonly MainContext db;
        public TicketService(IBaseRepository<MainContext, TicketGroup> ticketRepository,
            IMessageGeneratorService messageGeneratorService, ImageManagerService imageManagerService, MainContext db)
        {
            this.ticketRepository = ticketRepository;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
            this.db = db;
        }

        #region Common
        public PagedListResponse<TicketGroup> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<TicketGroup>();
            var list = ticketRepository.Table.GetPaged(model);
            response.SetData(list);

            return response;
        }
        public TicketGroup Get(int id)
        {
            return ticketRepository.Get().FirstOrDefault(s => s.Id == id);
        }
        public GetResponse<TicketGroup> GetResponse(int id)
        {
            var data = ticketRepository.Get().FirstOrDefault(s => s.Id == id);
            var response = new GetResponse<TicketGroup>();
            response.SetData(data);

            return response;
        }
        public IQueryable<TicketGroup> GetList()
        {
            return ticketRepository.Table;
        }
        public ListResponse<TicketGroup> GetListResponse()
        {
            var list = ticketRepository.Table.ToList();
            var response = new ListResponse<TicketGroup>();
            response.SetData(list);

            return response;
        }
        public EmptyResponse UpdateResponse(TicketGroup value)
        {
            ticketRepository.Table.Update(value);
            var dbResult = ticketRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = ticketRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("ticket", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            ticketRepository.Table.Remove(data);

            var dbResult = ticketRepository.Save();
            var response = new EmptyResponse(dbResult);

            TicketMessage ticketMessage = new TicketMessage();
            var ImageName = ticketMessage.ImageName;
            if (response.Success)
                imageManagerService.DeleteTicketImage(ImageName);

            return response;
        }
        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(ticketRepository.Table.Count());
            return response;
        }
        #endregion

        /*Ticket Group and Group Message for User/Admin  */
        public SendResponse SendResponse(TicketGroup value)
        {
            value.CreatedDate = DateTime.Now;

            if (value.Id == 0)
            {
                ticketRepository.Create(value);
                var dbResult = ticketRepository.Save();
                var response = new SendResponse(dbResult);
                response.SetData(value.Id);
                return response;
            }
            else
            {
                ticketRepository.Update(value);
                var dbResult = ticketRepository.Save();
                return new SendResponse(dbResult);
            }
        }

        #region User Page
        public ListResponse<TicketGroup> GetMessageGroups(int userId)
        {
            var ticketGroupList = ticketRepository.Table
                .Where(s => s.UserId == userId)
                .Include(s => s.TicketMessages)
                .ToList();

            var response = new ListResponse<TicketGroup>();
            response.SetData(ticketGroupList);

            return response;
        }
       
        public ListResponse<TicketMessage> GetTicketMessages(int groupId)
        {
            var ticketMessageList = ticketRepository.Table
                .Where(s => s.Id == groupId)
                .Include(s => s.TicketMessages)
                .FirstOrDefault().TicketMessages;

            var response = new ListResponse<TicketMessage>();
            response.SetData(ticketMessageList);

            return response;
        }
      
        public GetResponse<string> GetTicketImage(int messageId)
        {
            var imageName = db.TicketMessages
                .Where(s => s.Id == messageId)
                .FirstOrDefault().ImageName;

            var imagePath = imageManagerService.GetTicketImage(imageName);

            var response = new GetResponse<string>();
            response.SetData(imagePath);

            return response;
        }
      
        public CreateResponse CreateResponse(TicketGroup value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}