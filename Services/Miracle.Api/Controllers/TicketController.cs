using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;
using Miracle.Api.Responses.Ticket;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Ticket), ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly ImageManagerService imageManagerService;
        private readonly DataHelper dataHelper;
        private readonly IConfiguration configuration;
        private string[] contentTypes = new string[3] { "image/jpg", "image/jpeg", "image/png" };

        public TicketController(ITicketService ticketService, ImageManagerService imageManagerService,
            DataHelper dataHelper, IConfiguration configuration)
        {
            this.ticketService = ticketService;
            this.imageManagerService = imageManagerService;
            this.dataHelper = dataHelper;
            this.configuration = configuration;
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.User), Route(CRUDRoutes.GetById)]
        public GetResponse<TicketGroup> GetById(int id)
        {
            return ticketService.GetResponse(id);
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<TicketGroup> GetListAll()
        {
            var list = ticketService.GetList();
            var response = new ListResponse<TicketGroup>();
            response.SetData(list);

            return response;
        }

        [HttpPost, MiracleAuthorize, Route(CRUDRoutes.List)]
        public PagedListResponse<TicketGroup> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return ticketService.GetPagedListResponse(model);
            }
            return new PagedListResponse<TicketGroup>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return ticketService.GetCountResponse();
        }
        #endregion

        [HttpPost, MiracleAuthorize, Route(CRUDRoutes.Create)]
        public async Task<SendResponse> Send([FromForm] TicketGroupModel model)
        {

            if (model.ImageName != null && !contentTypes.Contains(model.ImageName.ContentType))
                ModelState.AddModelError("TicketImage to Uplaod", "Allowed Extensions: Jpg, Jpeg, Png");

            if (model.ImageName?.Length > 2000000)
                ModelState.AddModelError("TicketImage to Uplaod", "Image size must be smaller than 2MB");

            var adminName = "";
            var role = this.GetRoles();
            if (role.Contains("Admin") || role.Contains("SD"))
            {
                adminName = this.GetUsername();
            }

            var userName = "";
            if (role.Contains("User"))
            {
                userName = this.GetUsername();
            }


            if (ModelState.IsValid)
            {
                var userId = this.GetId();
                var isAdmin = this.GetRoles().Any(s => s == "Admin");

                if (model.Id == 0)
                {
                    TicketGroup ticketGroup = new TicketGroup();

                    ticketGroup.UserId = userId;

                    ticketGroup.SelectedProduct = model.SelectedProduct;
                    ticketGroup.Title = model.Title;

                    var response = ticketService.SendResponse(ticketGroup);

                    var ticketMessage = new TicketMessage()
                    {
                        Message = model.Message,
                        CreatedDate = DateTime.Now,
                        IsAdmin = isAdmin,
                        TicketGroupId = response.Id,
                        AdminName = adminName,
                        UserName = userName
                    };
                    
                    ticketGroup.IsClosed = true;

                    var result = dataHelper.FieldBinder(model, ticketGroup);

                    if (!result)
                        return new SendResponse(dataHelper.Errors);

                    if (model.ImageName != null)
                        ticketMessage.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageName.FileName);

                    if (response.Success && ticketMessage.ImageName != null)
                        await imageManagerService.SaveTicketImage(ticketMessage.ImageName, model.ImageName);

                    var group = ticketService.Get(response.Id);
                    group.Id = response.Id;
                    group.TicketMessages.Add(ticketMessage);

                    response = ticketService.SendResponse(group);

                    return response;
                }
                else
                {
                    var ticketGroup = ticketService.Get(model.Id);

                    var ticketMessage = new TicketMessage()
                    {
                        Message = model.Message,
                        CreatedDate = DateTime.Now,
                        IsAdmin = isAdmin,
                        TicketGroupId = model.Id,
                        AdminName = adminName,
                        UserName = userName
                    };

                    if (model.ImageName != null)
                        ticketMessage.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageName.FileName);

                    ticketGroup.TicketMessages.Add(ticketMessage);

                    var response = ticketService.SendResponse(ticketGroup);

                    if (response.Success && ticketMessage.ImageName != null)
                        await imageManagerService.SaveTicketImage(ticketMessage.ImageName, model.ImageName);

                    return response;
                }
            }
            return new SendResponse(this.GetModelStateErrors());
        }

        #region UserPage

        [HttpGet, MiracleAuthorize(Roles = Roles.User), Route(TicketRoutes.GetMessageGroups)]
        public ListResponse<TicketGroup> GetMessageGroups()
        {
            var userId = this.GetId();

            TicketGroup ticketGroup = new TicketGroup();

            if (ticketGroup.Id < 0)
                ModelState.AddModelError("TicketGroup", "No Recorded");

            if (ModelState.IsValid)
                return ticketService.GetMessageGroups(userId);

            return new ListResponse<TicketGroup>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.GetTicketMessages)]
        public ListResponse<TicketMessage> GetTicketMessages(int groupId)
        {
            if (ModelState.IsValid)
                return ticketService.GetTicketMessages(groupId);

            return new ListResponse<TicketMessage>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.GetImage)]
        public string GetTicketImage(int messageId)
        {
            return ticketService.GetTicketImage(messageId).Data;
        }

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.GetUserInfo)]
        public GetResponse<TicketUserModel> GetUserInfo([FromRoute] int userId)
        {
            Core.Api.Database.MainContext coreContext = new Core.Api.Database.MainContext();

            var user = coreContext.Users
                .Where(s => s.Id == userId)
                .Include(s => s.Company)
                .FirstOrDefault(s => s.Id == userId);

            var response = new GetResponse<TicketUserModel>();
            response.SetData(new TicketUserModel()
            {
                UserName = user.Username,
                Company = user.Company.Name,
                Id = user.Id
            });

            return response;
        }

        #endregion

        #region Admin

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.GetGroupReadInfo)]
        public EmptyResponse GetGroupReadInfo(int groupId)
        {
            var group = ticketService.Get(groupId);
            group.IsRead = true;

            ticketService.UpdateResponse(group);
            return new EmptyResponse();
        }

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.GetGroupCloseInfo)]
        public GetResponse<object> GetGroupCloseInfo(int groupId)
        {
            var group = ticketService.Get(groupId);
            var isClosed = group.IsClosed;

            return new GetResponse<object>(isClosed);
        }

        [HttpGet, MiracleAuthorize, Route(TicketRoutes.IsClosedChange)]
        public EmptyResponse IsClosedChange(int groupId)
        {
            var group = ticketService.Get(groupId);
            group.IsClosed = !group.IsClosed;

            ticketService.UpdateResponse(group);

            return new EmptyResponse();
        }

        #endregion
    }
}