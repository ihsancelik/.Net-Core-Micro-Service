using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.Priority;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Priority), ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityService priorityService;
        private readonly DataHelper dataHelper;
        public PriorityController(IPriorityService priorityService)
        {
            this.priorityService = priorityService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Priority> GetById(int id)
        {
            var data = priorityService.Get(id);

            var response = new GetResponse<Priority>();
            response.SetData(data);

            return response;
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<Priority> GetListAll()
        {
            var priorityList = priorityService.GetList();

            var response = new ListResponse<Priority>();
            response.SetData(priorityList);

            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<Priority> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return priorityService.GetPagedListResponse(model);
            }

            return new PagedListResponse<Priority>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(PriorityModel model)
        {
            if (ModelState.IsValid)
            {
                Priority priority = new Priority();

                var result = dataHelper.FieldBinder(model, priority);
                return result ? priorityService.CreateResponse(priority) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(PriorityModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = priorityService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Priority value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? priorityService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return priorityService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return priorityService.GetCountResponse();
        }
        #endregion

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(PriorityRoutes.GetPriorityByVersion)]
        public GetResponse<Priority> GetResponseByVersion(int productId, int versionInfoId)
        {
            return priorityService.GetResponseByVersion(productId, versionInfoId);
        }
    }
}