using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.ProductTag;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.ProductTag), ApiController]
    public class ProductTagController : ControllerBase
    {
        private readonly IProductTagService productTagService;
        private readonly DataHelper dataHelper;

        public ProductTagController(IProductTagService productTagService)
        {
            this.productTagService = productTagService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.GetById)]
        public GetResponse<ProductTag> GetById(int id)
        {
            return productTagService.GetResponse(id);
        }

        [HttpGet, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.ListAll)]
        public ListResponse<ProductTag> GetListAll()
        {
            var list = productTagService.GetList();
            var response = new ListResponse<ProductTag>();
            response.SetData(list);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.List)]
        public PagedListResponse<ProductTag> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return productTagService.GetPagedListResponse(model);
            }
            return new PagedListResponse<ProductTag>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(ProductTagModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new ProductTag();

                var result = dataHelper.FieldBinder(model, data);
                return result ? productTagService.CreateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(ProductTagModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = productTagService.Get(id);

                if (data == null)
                {
                    var message = MessageGenerator.Generate("ProductTag value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? productTagService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles.SD, Roles.Admin), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return productTagService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return productTagService.GetCountResponse();
        }
        #endregion
    }
}
