using Microsoft.EntityFrameworkCore;
using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Models.Product;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<MainContext, Product> productRepository;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;
        private readonly HTTPManagerService httpManagerService;

        public ProductService(IBaseRepository<MainContext, Product> productRepository,
            IMessageGeneratorService messageGeneratorService,
            ImageManagerService imageManagerService,
            HTTPManagerService httpManagerService)
        {
            this.productRepository = productRepository;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
            this.httpManagerService = httpManagerService;
        }

        public Product Get(int id)
        {
            return productRepository.Get().Include(p => p.ProductDetails).FirstOrDefault(p => p.Id == id);
        }
        public IQueryable<Product> GetList()
        {
            return productRepository.Table.Where(p => p.IsActive);
        }

        #region IBaseResponseService
        public PagedListResponse<Product> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<Product>();
            var list = productRepository.Table.Include(p => p.ProductDetails).GetPaged(model);
            response.SetData(list);
            return response;
        }
        public GetResponse<Product> GetResponse(int id)
        {
            var data = productRepository.Get().Include(p => p.ProductDetails).FirstOrDefault(p => p.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Product", MessageGeneratorActions.NotFound);
                return new GetResponse<Product>(message);
            }

            var response = new GetResponse<Product>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Product> GetListResponse()
        {
            var list = productRepository.Table.ToList();
            var response = new ListResponse<Product>();
            response.SetData(list);
            return response;
        }
        public CreateResponse CreateResponse(Product value)
        {

            var exist = productRepository.Table.Any(s => s.Name == value.Name && s.VersionId == value.VersionId);
            if (exist)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Product", MessageGeneratorActions.Exist);
                return new CreateResponse(message);
            }

            productRepository.Create(value);

            var dbResult = productRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);
            return response;
        }
        public EmptyResponse UpdateResponse(Product value)
        {
            productRepository.Table.Update(value);
            var dbResult = productRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = productRepository.Get().FirstOrDefault(p => p.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Product", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            productRepository.Table.Remove(data);
            var dbResult = productRepository.Save();

            var response = new EmptyResponse(dbResult);

            if (response.Success)
                imageManagerService.DeleteNewsImage(data.ImageName);

            return response;
        }
        #endregion

        public async Task<ListResponse<object>> GetProductsOutSource()
        {
            var response = await httpManagerService.GetAsync<ListResponse<object>>("product/getProducts");
            return response != null ? response : new ListResponse<object>();
        }
        public GetResponse<string> GetProductImagePath(int id)
        {
            var imageName = productRepository.Get().FirstOrDefault(p => p.Id == id).ImageName;
            var imagePath = imageManagerService.GetProductImage(imageName);
            var response = new GetResponse<string>();
            response.SetData(imagePath);
            return response;
        }
        public ListResponse<Product> GetProductByTag(string tag)
        {
            var list = productRepository.Table
                .Where(p => p.Tag == tag)
                .Include(p => p.ProductDetails)
                .ToList();
            var response = new ListResponse<Product>();
            response.SetData(list);
            return response;
        }
        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(productRepository.Table.Count());
            return response;
        }

        public async Task<EmptyResponse> AddUserProductAsync(int userId, string authToken, string tag, int versionId)
        {
            var response = await httpManagerService.GetAsync<EmptyResponse>($"user/addProductOutSource/{userId}/{tag}/{versionId}", authToken);
            return response != null ? response : new EmptyResponse();
        }
    }
}