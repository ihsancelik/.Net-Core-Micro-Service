using Library.Responses.Common;
using Microsoft.AspNetCore.Http;
using Miracle.Core.Api.Database.Models;

namespace Miracle.Core.Api.Services
{
    public interface IProductService : IBaseResponseService<Product>
    {
        public EmptyResponse AddVersion(int productId, int versionInfoId, int priorityId);
        public EmptyResponse RemoveVersion(int productId, int versionInfoId);
        public EmptyResponse AddSetup(int platformId, int productId, int versionInfoId, IFormFile file);
        public GetResponseObject ExistSetup(int platformId, int productId, int versionInfoId);
        public EmptyResponse AddModule(int productId, int moduleId);
        public EmptyResponse RemoveModule(int productId, int moduleId);
    }
}
