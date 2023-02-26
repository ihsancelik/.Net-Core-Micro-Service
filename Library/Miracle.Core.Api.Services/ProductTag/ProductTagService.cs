using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services.Extensions;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public class ProductTagService : IProductTagService
    {
        private DatabaseResponse dbResponse;
        private readonly MainContext db;

        public ExceptionManager ExceptionManager { get; set; }
        public ProductTagService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public ProductTag Get(int id)
        {
            var data = db.ProductTags.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("ProductTag", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<ProductTag> GetList()
        {
            return db.ProductTags;
        }
        public PagedResponse<ProductTag> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.ProductTags.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(ProductTag model)
        {
            var isExist = db.ProductTags.Any(s => s.Tag == model.Tag);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("ProductTag", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.ProductTags.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(ProductTag model)
        {
            var isExist = db.ProductTags.Any(s => s.Id != model.Id && s.Tag == model.Tag);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("ProductTag", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.ProductTags.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.ProductTags.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("ProductTag", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.ProductTags.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.ProductTags.Count();
        }
        #endregion

        #region Response
        public GetResponse<ProductTag> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<ProductTag>(ExceptionManager.Exceptions);

            var response = new GetResponse<ProductTag>();
            response.SetData(data);
            return response;
        }
        public ListResponse<ProductTag> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<ProductTag>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<ProductTag> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<ProductTag>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(ProductTag model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(ProductTag model)
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

    }
}
