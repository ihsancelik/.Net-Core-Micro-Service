using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services.Extensions;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public class ProductModuleService : IProductModuleService
    {
        private DatabaseResponse dbResponse;
        private readonly MainContext db;
        public ExceptionManager ExceptionManager { get; set; }
        public ProductModuleService(MainContext db)
        {
            this.db = db;
            ExceptionManager = new ExceptionManager();
            dbResponse = new DatabaseResponse();
        }


        #region Common
        public ProductModule Get(int id)
        {
            var data = db.ProductModules
                .Include(s => s.Product_Modules)
                .FirstOrDefault(s => s.Id == id);

            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Product", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<ProductModule> GetList()
        {
            return db.ProductModules
                .Include(s => s.Product_Modules);
        }
        public PagedResponse<ProductModule> GetPagedList(PaginationParameterModel model)
        {
            return db.ProductModules.GetPaged(model);
        }
        public DatabaseResponse Create(ProductModule model)
        {
            var isExist = db.ProductModules.Any(s => s.Name == model.Name);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Product Module", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.ProductModules.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(ProductModule model)
        {
            var isExist = db.ProductModules.Any(s => s.Id != model.Id && (s.Name == model.Name));

            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Product Module", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.ProductModules.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            ProductModule data = db.ProductModules.FirstOrDefault(s => s.Id == id);

            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Product Module", MessageGeneratorActions.NotFound));

            db.ProductModules.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
            {
                ExceptionManager.AddException(dbResponse.Exception);
            }

            return dbResponse;
        }
        public int Count()
        {
            return db.ProductModules.Count();
        }
        #endregion

        #region Response
        public GetResponse<ProductModule> GetResponse(int id)
        {
            var data = Get(id);
            if (data == null)
                return new GetResponse<ProductModule>(ExceptionManager.Exceptions);

            var response = new GetResponse<ProductModule>();
            response.SetData(data);
            return response;
        }
        public ListResponse<ProductModule> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<ProductModule>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<ProductModule> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<ProductModule>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(ProductModule model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(ProductModule model)
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

        public PagedListResponse<ProductModule> GetListByProductResponse(int productId, PaginationParameterModel paginationModel)
        {
            var response = new PagedListResponse<ProductModule>();

            var data = db.Product_Modules
                .Where(s => s.ProductId == productId)
                .Include(s => s.Module)
                .Select(s => s.Module)
                .GetPaged(paginationModel);

            response.SetData(data);
            return response;
        }
    }
}
