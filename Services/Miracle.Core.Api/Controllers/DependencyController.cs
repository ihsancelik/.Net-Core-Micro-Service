using Library.Helpers.Attributes;
using Library.Helpers.Constraints;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.AppLib;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.Services.Helpers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Dependency), MiracleAuthorize(Roles.SD), ApiController]
    public class DependencyController : ControllerBase
    {
        private readonly IAppLibService appLibService;
        private readonly AppLibManager appLibManager;
        private readonly DataHelper dataHelper;

        public DependencyController(IAppLibService appLibService, AppLibManager appLibManager)
        {
            this.appLibService = appLibService;
            this.appLibManager = appLibManager;
            dataHelper = new DataHelper();
        }


        #region Common
        [HttpGet, Route(CRUDRoutes.GetById)]
        public GetResponse<AppLib> GetById(int id)
        {
            if (ModelState.IsValid)
            {
                var data = appLibService.Get(id);
                var response = new GetResponse<AppLib>();
                response.SetData(data);

                return response;
            }

            return new GetResponse<AppLib>(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<AppLib> GetListAll()
        {
            var list = appLibService.GetList();
            var response = new ListResponse<AppLib>();
            response.SetData(list);
            return response;
        }

        [HttpPost, Route(CRUDRoutes.List)]
        public PagedListResponse<AppLib> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return appLibService.GetPagedListResponse(model);
            }

            return new PagedListResponse<AppLib>(this.GetModelStateErrors());
        }

        [HttpPost, Route(CRUDRoutes.Create)]
        public async Task<EmptyResponse> CreateAsync([FromForm] AppLibModel model)
        {
            if (ModelState.IsValid)
            {
                AppLib appLib = new AppLib();

                var result = dataHelper.FieldBinder(model, appLib);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var response = appLibService.CreateResponse(appLib);
                if (!response.Success)
                    return new EmptyResponse(response.ErrorList);

                await appLibManager.SaveAsync(model.LibName, model.LibFile);
                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromForm] AppLibModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = appLibService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("AppLib", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var response = appLibService.UpdateResponse(data);
                if (!response.Success)
                    return new EmptyResponse(response.ErrorList);

                if (model.LibFile == null)
                    return response;

                await appLibManager.SaveAsync(model.LibName, model.LibFile);
                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var data = appLibService.Get(id);
                if (data.IsActive)
                    return new EmptyResponse("Lib is active, cannot delete!");

                var libName = data.LibName;

                var response = appLibService.DeleteResponse(id);
                if (!response.Success)
                    return new EmptyResponse(response.ErrorList);

                appLibManager.Delete(libName);

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return appLibService.GetCountResponse();
        }
        #endregion

        [HttpGet, Route("active/{libName}/{active}")]
        public GetResponse<object> SetActivateStatus([FromRoute] string libName, [FromRoute] string active)
        {
            bool.TryParse(active, out bool isActive);

            var appLib = appLibService.GetList().FirstOrDefault(s => s.LibName == libName);
            if (appLib == null)
                return new GetResponse<object>("Null");

            var libFilePath = Path.Combine(ApiCorePathConstraints.LibFiles, libName);

            return null;
        }
    }
}