using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Models.Product;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Product), ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly DataHelper dataHelper;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;

        public ProductController(IProductService productService,
            DataHelper dataHelper,
            IMessageGeneratorService messageGeneratorService,
            ImageManagerService imageManagerService)
        {
            this.productService = productService;
            this.dataHelper = dataHelper;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
        }

        #region Common 

        [HttpGet, MiracleAuthorize, Route(CRUDRoutes.GetById)]
        public GetResponse<Product> GetById(int id)
        {
            return productService.GetResponse(id);
        }

        [HttpPost, Route(CRUDRoutes.List)]
        public PagedListResponse<Product> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return productService.GetPagedListResponse(model);
            }

            return new PagedListResponse<Product>(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<Product> GetListAll()
        {
            var productList = productService.GetList();
            var response = new ListResponse<Product>();
            response.SetData(productList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Create)]
        public async Task<CreateResponse> CreateAsync([FromForm] ProductModel model)
        {
            if (model.ProductImage == null)
            {
                ModelState.AddModelError("Image", "Image cannot be empty");
            }

            if (ModelState.IsValid)
            {
                Product product = new Product();
                var result = dataHelper.FieldBinder(model, product);

                if (!result)
                    return new CreateResponse(dataHelper.Errors);               


                product.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProductImage.FileName);

                var response = productService.CreateResponse(product);

                if (response.Success)
                    await imageManagerService.SaveProductImage(product.ImageName, model.ProductImage);

                return response;

            }

            return new CreateResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromRoute] int id, [FromForm] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var productImage = model.ProductImage;

                var data = productService.Get(id);
                if (data == null)
                {
                    var message = messageGeneratorService.PrepareResponseMessage("Product", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var oldImageName = data.ImageName;

                if (productImage != null)
                {
                    data.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProductImage.FileName);
                }

                var response = productService.UpdateResponse(data);

                if (response.Success && productImage != null)
                {
                    await imageManagerService.UpdateProductImage(oldImageName, data.ImageName, model.ProductImage);
                }

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return productService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return productService.GetCountResponse();
        }

        #endregion

        #region ProductDetails
        [HttpPost, MiracleAuthorize, Route(ProductRoutes.AddProductDetail)]
        public EmptyResponse AddProductDetail([FromRoute] int productId, [FromBody] ProductDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var product = productService.Get(productId);

                if (model.Id != 0)
                {
                    var detail = product.ProductDetails.FirstOrDefault(d => d.Id == model.Id);
                    product.ProductDetails.Remove(detail);
                }
                product.ProductDetails.Add(new ProductDetail { Title = model.Title, Content = model.Content });

                return productService.UpdateResponse(product);
            }
            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize, Route(ProductRoutes.RemoveProductDetail)]
        public EmptyResponse RemoveProductDetail([FromRoute] int productId, [FromRoute] int productDetailId)
        {
            var product = productService.Get(productId);
            var detail = product.ProductDetails.FirstOrDefault(d => d.Id == productDetailId && d.ProductId == productId);

            product.ProductDetails.Remove(detail);

            return productService.UpdateResponse(product);
        }
        #endregion 

        #region OutSource

        [HttpGet, MiracleAuthorize, Route(ProductRoutes.GetProducts)]
        public async Task<ListResponse<object>> GetProductsOutSourceAsync()
        {
            return await productService.GetProductsOutSource();
        }

        [HttpGet, Route(ProductRoutes.GetImage)]
        public string GetProductImage(int id)
        {
            return productService.GetProductImagePath(id).Data;
        }

        [HttpGet, Route(ProductRoutes.GetByTag)]
        public ListResponse<Product> GetByTag(string tag)
        {
            if (ModelState.IsValid)
                return productService.GetProductByTag(tag);

            return new ListResponse<Product>(this.GetModelStateErrors());
        }

        #endregion
    }
}