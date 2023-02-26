using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Platform;
using Miracle.Core.Api.Services;
using System.Linq;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Platform), ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService platformService;
        private readonly DataHelper dataHelper;
        public PlatformController(IPlatformService platformService)
        {
            this.platformService = platformService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Platform> GetById(int id)
        {
            var platform = platformService.Get(id);
            var response = new GetResponse<Platform>(true);
            response.SetData(platform);

            return response;
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<Platform> GetList()
        {
            var platformList = platformService.GetList().ToList();
            var response = new ListResponse<Platform>();
            response.SetData(platformList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(PlatformModel model)
        {
            if (ModelState.IsValid)
            {
                Platform platform = new Platform();

                var result = dataHelper.FieldBinder(model, platform);
                return result ? platformService.CreateResponse(platform) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(PlatformModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = platformService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Platform value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? platformService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return platformService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return platformService.GetCountResponse();
        }
        #endregion

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(PlatformRoutes.GetListByProductId)]
        public ListResponse<Platform> GetListByProductId(int productId)
        {
            return platformService.GetListByProductId(productId);
        }
    }
}