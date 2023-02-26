using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.Product;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Product), ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly DataHelper dataHelper;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Product> GetById(int id)
        {
            var product = productService.Get(id);
            var response = new GetResponse<Product>();
            response.SetData(product);

            return response;
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<Product> GetListAll()
        {
            var productList = productService.GetList();
            var response = new ListResponse<Product>();
            response.SetData(productList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<Product> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return productService.GetPagedListResponse(model);
            }
            return new PagedListResponse<Product>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                var result = dataHelper.FieldBinder(model, product);

                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                foreach (int platformId in model.PlatformIdList)
                {
                    product.Platform_Products.Add(new Platform_Product
                    {
                        ProductId = product.Id,
                        PlatformId = platformId
                    });
                }


                return productService.CreateResponse(product);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse UpdateProduct(ProductModel model, [FromRoute] int id)
        {
            EmptyResponse response = null;

            if (ModelState.IsValid)
            {
                Product product = new Product();

                var data = productService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("Product", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                {
                    response = new EmptyResponse(false);
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                data.Platform_Products.Clear();
                foreach (int platformId in model.PlatformIdList)
                {
                    var platformProduct = new Platform_Product()
                    {
                        ProductId = product.Id,
                        PlatformId = platformId
                    };
                    data.Platform_Products.Add(platformProduct);
                }


                return productService.UpdateResponse(data);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse DeleteProduct(int id)
        {
            if (ModelState.IsValid)
                return productService.DeleteResponse(id);


            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return productService.GetCountResponse();
        }
        #endregion

        #region Product Version
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.AddVersion)]
        public EmptyResponse AddVersion(int productId, int versionInfoId, int priorityId)
        {
            return ModelState.IsValid ?
                productService.AddVersion(productId, versionInfoId, priorityId) :
                new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.RemoveVersion)]
        public EmptyResponse RemoveVersion(int productId, int versionInfoId)
        {
            return ModelState.IsValid ?
                productService.RemoveVersion(productId, versionInfoId) : new EmptyResponse(this.GetModelStateErrors());
        }

        #endregion

        #region Product Module

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.AddModule)]
        public EmptyResponse AddModule(int productId, int moduleId)
        {
            return ModelState.IsValid ?
                productService.AddModule(productId, moduleId) :
                new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.RemoveModule)]
        public EmptyResponse RemoveModule(int productId, int moduleId)
        {
            return ModelState.IsValid ?
                productService.RemoveModule(productId, moduleId) : new EmptyResponse(this.GetModelStateErrors());
        }
        #endregion

        #region Setup
        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.AddSetup), RequestSizeLimit(300000000)]
        public EmptyResponse AddSetups(int platformId, int productId, int versionInfoId, [FromForm] IFormFile file)
        {
            return ModelState.IsValid ?
                productService.AddSetup(platformId, productId, versionInfoId, file)
                : new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(ProductRoutes.ExistSetup)]
        public GetResponseObject ExistSetup(int platformId, int productId, int versionInfoId)
        {
            if (ModelState.IsValid)
                return productService.ExistSetup(platformId, productId, versionInfoId);

            return new GetResponseObject(this.GetModelStateErrors());
        }
        #endregion
    }
}