using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.LiveTicket), ApiController]
    public class LiveTicketController : Controller
    {
        private readonly ILiveTicketService liveService;

        public LiveTicketController(ILiveTicketService liveService)
        {
            this.liveService = liveService;
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<LiveChat> GetListAll()
        {
            var list = liveService.GetList();

            var response = new ListResponse<LiveChat>();
            response.SetData(list);

            return response;
        }

        [HttpGet, Route(LiveTicketRoutes.GetContents)]
        public ListResponse<LiveChatContent> GetContents(string roomName)
        {
            return liveService.GetContents(roomName);
        }

        [HttpGet, Route(LiveTicketRoutes.GetChats)]
        public ListResponse<LiveChat> GetChats(string roomName)
        {
            return liveService.GetChats(roomName);
        }

        [HttpGet, Route(LiveTicketRoutes.GetCustomerName)]
        public GetResponse<string> GetCustomerName(string roomName)
        {
            return liveService.GetCustomerName(roomName);
        }
    }
}