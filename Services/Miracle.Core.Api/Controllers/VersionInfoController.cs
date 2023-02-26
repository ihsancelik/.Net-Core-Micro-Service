using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.VersionInfo;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.VersionInfo), ApiController]
    public class VersionInfoController : ControllerBase
    {
        private readonly IVersionInfoService versionInfoService;
        private readonly DataHelper dataHelper;

        public VersionInfoController(IVersionInfoService versionInfoService)
        {
            this.versionInfoService = versionInfoService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<VersionInfo> GetById(int id)
        {
            return versionInfoService.GetResponse(id);
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<VersionInfo> GetListAll()
        {
            var companyList = versionInfoService.GetList();
            var response = new ListResponse<VersionInfo>();
            response.SetData(companyList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<VersionInfo> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return versionInfoService.GetPagedListResponse(model);
            }
            return new PagedListResponse<VersionInfo>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(VersionInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new VersionInfo();

                var result = dataHelper.FieldBinder(model, data);
                return result ? versionInfoService.CreateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(VersionInfoModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = versionInfoService.Get(id);

                if (data == null)
                {
                    var message = MessageGenerator.Generate("VersionInfo value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? versionInfoService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return versionInfoService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return versionInfoService.GetCountResponse();
        }
        #endregion

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(VersionInfoRoutes.GetVersionListByProductId)]
        public PagedListResponse<VersionInfo> GetList([FromRoute] int productId, [FromBody] PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return versionInfoService.GetListByProductResponse(productId, model);
            }

            return new PagedListResponse<VersionInfo>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(VersionInfoRoutes.GetVersionListByUserProduct)]
        public ListResponse<VersionInfo> GetList([FromRoute] int productId)
        {
            if (ModelState.IsValid)
            {
                return versionInfoService.GetListByUserProduct(productId);
            }

            return new ListResponse<VersionInfo>(this.GetModelStateErrors());
        }
    }
}