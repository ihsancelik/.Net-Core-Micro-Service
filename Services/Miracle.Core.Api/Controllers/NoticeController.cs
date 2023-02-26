using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Notice;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Notice), MiracleAuthorize(Roles = Roles.SD), ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeService noticeService;
        private readonly DataHelper dataHelper;
        public NoticeController(INoticeService noticeService)
        {
            this.noticeService = noticeService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Notice> GetById(int id)
        {
            var notice = noticeService.Get(id);
            var response = new GetResponse<Notice>();
            response.SetData(notice);

            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<Notice> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return noticeService.GetPagedListResponse(model);
            }
            return new PagedListResponse<Notice>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(NoticeModel model)
        {
            if (ModelState.IsValid)
            {
                Notice notice = new Notice();

                var result = dataHelper.FieldBinder(model, notice);
                return result ? noticeService.CreateResponse(notice) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(NoticeModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = noticeService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Notice value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);

                return result ? noticeService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return noticeService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return noticeService.GetCountResponse();
        }
        #endregion
    }
}