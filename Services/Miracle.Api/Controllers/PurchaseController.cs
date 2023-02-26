using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Purchase), MiracleAuthorize(Roles = Roles.Admin), ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        #region Common
        [HttpGet, Route(CRUDRoutes.GetById)]
        public GetResponse<Purchase> GetById(int id)
        {
            return purchaseService.GetResponse(id);
        }

        [HttpPost, Route(CRUDRoutes.List)]
        public PagedListResponse<Purchase> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return purchaseService.GetPagedListResponse(model);
            }

            return new PagedListResponse<Purchase>(this.GetModelStateErrors());
        }

        [HttpDelete, Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return purchaseService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return purchaseService.GetCountResponse();
        }

        #endregion
    }
}