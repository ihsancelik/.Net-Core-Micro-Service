using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.Role;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Role), ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly DataHelper dataHelper;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Role> GetById(int id)
        {
            if (ModelState.IsValid)
            {
                var role = roleService.Get(id);
                var response = new GetResponse<Role>();
                response.SetData(role);

                return response;
            }

            return new GetResponse<Role>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<Role> GetListAll()
        {
            var roleList = roleService.GetList();
            var response = new ListResponse<Role>();
            response.SetData(roleList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<Role> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return roleService.GetPagedListResponse(model);
            }

            return new PagedListResponse<Role>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role();

                var result = dataHelper.FieldBinder(model, role);
                return result ? roleService.CreateResponse(role) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(RoleModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = roleService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Role", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? roleService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return roleService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return roleService.GetCountResponse();
        }
        #endregion
    }
}