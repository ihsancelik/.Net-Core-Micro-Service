using Adapter.Miracle.Api.Services;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Library.Helpers.Constraints.PlatformConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Adapter.Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Product), ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServiceAdapter productService;

        public ProductController(IProductServiceAdapter productService)
        {
            this.productService = productService;
        }

        [HttpGet, Route(ProductRoutes.GetProducts)]
        public ListResponse<object> GetProducts()
        {
            if (ModelState.IsValid)
                return productService.GetProducts();

            return new ListResponse<object>(this.GetModelStateErrors());
        }

        [HttpGet, AllowAnonymous, Route(ProductRoutes.GetMiracleWorld)]
        public string GetMW([FromRoute] string platform)
        {
            string path = "https://test.com/api/Files/MiracleWorld/MiracleWorld.";

            if (platform == Windows) path += "exe";
            else if (platform == Mac) path += "dmg";
            else path += "deb";

            return path;
        }
    }
}