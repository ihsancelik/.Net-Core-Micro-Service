using Library.Helpers.Attributes;
using Library.Helpers.Constraints;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Package.Manager.Api.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Package.Manager.Api.Controllers
{
    [ApiController, Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost, Route("get"), AllowAnonymous]
        public async Task<ListResponse<PackageModel>> GetAsync(GetProductInfoModel model)
        {
            var response = new ListResponse<PackageModel>();
            if (ModelState.IsValid)
            {
                var result = await productService.Get(model.ProductTag, model.Platform, model.Version);
                if (!result)
                {
                    response.AddErrorList(productService.Exception);
                    return response;
                }

                response.SetData(productService.ProductInfo.Packages);
                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route("create"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        public async Task<GetResponse<string>> CreateAsync(ProductInfo model)
        {
            var response = new GetResponse<string>();
            if (ModelState.IsValid)
            {
                var result = await productService.Create(model);
                if (!result)
                {
                    response.AddErrorList(productService.Exception);
                    return response;
                }

                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }
    }

    public class GetProductInfoModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string ProductTag { get; set; }
        [Required]
        public string Version { get; set; }
    }
}