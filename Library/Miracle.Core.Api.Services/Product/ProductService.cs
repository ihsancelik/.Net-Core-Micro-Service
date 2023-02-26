using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services.Extensions;
using Miracle.Core.Api.Services.Helpers;
using System;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public class ProductService : IProductService
    {
        private const string ZIPEXTENSION = "zip";
        private readonly MainContext db;
        private readonly SetupManagerService setupManagerService;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }

        public ProductService(MainContext db,
            SetupManagerService setupManagerService)
        {
            this.db = db;
            this.setupManagerService = setupManagerService;
            ExceptionManager = new ExceptionManager();
            dbResponse = new DatabaseResponse();
        }

        #region Common
        public Product Get(int id)
        {
            var data = db.Products
                .Include(s => s.Platform_Products)
                .Include(s => s.User_Products)
                .FirstOrDefault(s => s.Id == id);

            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Product", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<Product> GetList()
        {
            return db.Products
                .Include(s => s.User_Products);
        }
        public PagedResponse<Product> GetPagedList(PaginationParameterModel model)
        {
            return db.Products.GetPaged(model);
        }
        public DatabaseResponse Create(Product model)
        {
            var isExist = db.Products.Any(s => s.Name == model.Name && s.Tag == model.Tag);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Product", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            model.PublishDate = DateTime.Now;

            db.Products.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(Product model)
        {
            var isExist = db.Products.Any(s => s.Id != model.Id && (s.Name == model.Name && s.Tag == model.Tag));

            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Product", MessageGeneratorActions.Exist));
                return dbResponse;
            }
            var productPublishDate = db.Products.Where(s => s.Id == model.Id).Select(s => s.PublishDate).FirstOrDefault();
            model.PublishDate = productPublishDate;

            db.Products.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {

            Product data = db.Products.FirstOrDefault(s => s.Id == id);

            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Product", MessageGeneratorActions.NotFound));

            db.Products.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
            {
                ExceptionManager.AddException(dbResponse.Exception);
            }

            return dbResponse;
        }
        public int Count()
        {
            return db.Products.Count();
        }
        #endregion

        #region Response
        public GetResponse<Product> GetResponse(int id)
        {
            var data = Get(id);
            if (data == null)
                return new GetResponse<Product>(ExceptionManager.Exceptions);

            var response = new GetResponse<Product>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Product> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<Product>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<Product> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<Product>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Product model)
        {
            Create(model);

            model.PublishDate = DateTime.Now;

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(Product model)
        {
            Update(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            Delete(id);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);
            return new EmptyResponse(dbResponse);
        }
        public GetResponse<object> GetCountResponse()
        {
            int count = Count();
            var response = new GetResponse<object>();
            response.SetData(count);
            return response;
        }
        #endregion

        #region Version
        public EmptyResponse AddVersion(int productId, int versionInfoId, int priorityId)
        {
            var product = db.Products
                .Where(s => s.Id == productId)
                .Include(s => s.ProductSettings)
                .ThenInclude(s => s.VersionInfo)
                .FirstOrDefault();

            var remove = product.ProductSettings.FirstOrDefault(s => s.VersionInfoId == versionInfoId);
            product.ProductSettings.Remove(remove);

            product.ProductSettings.Add(new ProductSetting()
            {
                ProductId = productId,
                VersionInfoId = versionInfoId,
                PriorityId = priorityId
            });

            db.Products.Update(product);
            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
        public EmptyResponse RemoveVersion(int productId, int versionInfoId)
        {
            var product = db.Products
                .Where(s => s.Id == productId)
                .Include(s => s.Platform_Products)
                .ThenInclude(s => s.Platform)
                .Include(s => s.ProductSettings)
                .ThenInclude(s => s.VersionInfo)
                .FirstOrDefault();

            var product_VersionInfo = product.ProductSettings
                .FirstOrDefault(s => s.ProductId == productId && s.VersionInfoId == versionInfoId);
            product.ProductSettings.Remove(product_VersionInfo);

            var deleted = db.User_Versions
                .Include(s => s.VersionInfo)
                .Where(s => s.VersionInfo.Id == product_VersionInfo.VersionInfoId)
                .ToList();
            db.User_Versions.RemoveRange(deleted);

            db.Products.Update(product);
            var dbResult = db.Save();

            if (dbResult.Success)
            {
                var platformIdList = product.Platform_Products.Select(s => s.Platform.Id);
                foreach (int platformId in platformIdList)
                    RemoveSetup(platformId, productId, versionInfoId);

            }

            return new EmptyResponse(dbResult);
        }
        #endregion

        #region Module
        public EmptyResponse AddModule(int productId, int moduleId)
        {
            var product = db.Products
                .Where(s => s.Id == productId)
                .Include(s => s.Product_Modules)
                .FirstOrDefault();

            var remove = product.Product_Modules.FirstOrDefault(s => s.ProductModuleId == moduleId);
            product.Product_Modules.Remove(remove);

            product.Product_Modules.Add(new Product_Module()
            {
                ProductId = productId,
                ProductModuleId = moduleId
            });

            db.Products.Update(product);
            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
        public EmptyResponse RemoveModule(int productId, int moduleId)
        {
            var product = db.Products
                .Where(s => s.Id == productId)
                .Include(s => s.Product_Modules)
                .FirstOrDefault();

            var product_Module = product.Product_Modules
                .FirstOrDefault(s => s.ProductId == productId && s.ProductModuleId == moduleId);
            product.Product_Modules.Remove(product_Module);

            db.Products.Update(product);
            var dbResult = db.Save();

            return new EmptyResponse(dbResult);
        }
        #endregion

        #region Setup
        public EmptyResponse AddSetup(int platformId, int productId, int versionInfoId, IFormFile file)
        {
            var setupInfo = db.SetupInfos.FirstOrDefault(s => s.PlatformId == platformId &&
                                                              s.ProductId == productId &&
                                                              s.VersionInfoId == versionInfoId);

            var isProduct = !db.Products
                .Where(s => s.Id == productId)
                .Select(s => s.IsPlugin)
                .FirstOrDefault();

            if (setupInfo != null)
            {
                var fileName = setupInfo.Name + "." + setupInfo.Extension;
                db.SetupInfos.Remove(setupInfo);
                setupManagerService.Delete(fileName, isProduct);
            }

            var newSetupInfo = (new SetupInfo()
            {
                Name = Guid.NewGuid().ToString(),
                Extension = ZIPEXTENSION,
                Path = setupManagerService.GetPath(isProduct),
                PlatformId = platformId,
                ProductId = productId,
                VersionInfoId = versionInfoId
            });
            db.SetupInfos.Add(newSetupInfo);
            var dbResult = db.Save();
            if (dbResult.Success)
            {
                var newFileName = newSetupInfo.Name + "." + newSetupInfo.Extension;
                setupManagerService.Save(file, newFileName, isProduct);
            }

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse RemoveSetup(int platformId, int productId, int versionInfoId)
        {
            var setupInfo = db.SetupInfos.FirstOrDefault(s => s.PlatformId == platformId &&
                                                              s.ProductId == productId &&
                                                              s.VersionInfoId == versionInfoId);
            if (setupInfo == null)
                return new EmptyResponse(true, "No setup file.");

            var fileName = setupInfo.Name + "." + setupInfo.Extension;

            var isProduct = !db.Products
                .Where(s => s.Id == productId)
                .Select(s => s.IsPlugin)
                .FirstOrDefault();

            db.SetupInfos.Remove(setupInfo);
            var dbResult = db.Save();

            if (dbResult.Success)
                setupManagerService.Delete(fileName, isProduct);

            return new EmptyResponse(dbResult);
        }
        public GetResponseObject ExistSetup(int platformId, int productId, int versionInfoId)
        {
            var exist = db.SetupInfos.Any(s => s.PlatformId == platformId &&
                                     s.ProductId == productId &&
                                     s.VersionInfoId == versionInfoId);

            var response = new GetResponseObject();
            response.SetData(exist);
            return response;
        }

        #endregion
    }
}
