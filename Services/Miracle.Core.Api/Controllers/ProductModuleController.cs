using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.ProductModule;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.ProductModule), ApiController]
    public class ProductModuleController : ControllerBase
    {
        private readonly IProductModuleService productModuleService;
        private readonly DataHelper dataHelper;

        public ProductModuleController(IProductModuleService productModuleService)
        {
            this.productModuleService = productModuleService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<ProductModule> GetById(int id)
        {
            var productModule = productModuleService.Get(id);
            var response = new GetResponse<ProductModule>();
            response.SetData(productModule);

            return response;
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<ProductModule> GetListAll()
        {
            var productModuleList = productModuleService.GetList();
            var response = new ListResponse<ProductModule>();
            response.SetData(productModuleList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<ProductModule> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return productModuleService.GetPagedListResponse(model);
            }
            return new PagedListResponse<ProductModule>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse CreateProductModule(ProductModuleModel model)
        {
            if (ModelState.IsValid)
            {
                ProductModule productModule = new ProductModule();
                var result = dataHelper.FieldBinder(model, productModule);

                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                return productModuleService.CreateResponse(productModule);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse UpdateProduct(ProductModuleModel model, [FromRoute] int id)
        {
            EmptyResponse response = null;

            if (ModelState.IsValid)
            {
                var data = productModuleService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Product Module", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                {
                    response = new EmptyResponse(false);
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                return productModuleService.UpdateResponse(data);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse DeleteProduct(int id)
        {
            if (ModelState.IsValid)
                return productModuleService.DeleteResponse(id);

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return productModuleService.GetCountResponse();
        }
        #endregion

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(ProductModuleRoutes.GetListByProductId)]
        public PagedListResponse<ProductModule> GetList([FromRoute] int productId, [FromBody] PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return productModuleService.GetListByProductResponse(productId, model);
            }

            return new PagedListResponse<ProductModule>(this.GetModelStateErrors());
        }

    }
}