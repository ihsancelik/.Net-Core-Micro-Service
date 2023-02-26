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
    public class PlatformService : IPlatformService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }

        public PlatformService(MainContext db)
        {
            this.db = db;
            ExceptionManager = new ExceptionManager();
            dbResponse = new DatabaseResponse();
        }

        #region Common
        public Platform Get(int id)
        {
            var data = db.Platforms.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Platform", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<Platform> GetList()
        {
            return db.Platforms;
        }
        public PagedResponse<Platform> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Platforms.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(Platform model)
        {
            var isExist = db.Platforms.Any(s => s.Name == model.Name);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Platform", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Platforms.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(Platform model)
        {
            var isExist = db.Platforms.Any(s => s.Id != model.Id && s.Name == model.Name);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Platform", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Platforms.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Platforms.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Platform", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Platforms.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.Platforms.Count();
        }
        #endregion

        #region Response
        public GetResponse<Platform> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<Platform>(ExceptionManager.Exceptions);

            var response = new GetResponse<Platform>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Platform> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<Platform>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<Platform> GetPagedListResponse(PaginationParameterModel model)
        {
            var data = GetPagedList(model);
            var response = new PagedListResponse<Platform>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Platform model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(Platform model)
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

        public ListResponse<Platform> GetListByProductId(int productId)
        {
            var platforms = db.Platform_Products
                .Where(s => s.ProductId == productId)
                .Include(s => s.Platform)
                .Select(s => s.Platform)
                .ToList();

            var response = new ListResponse<Platform>();
            response.SetData(platforms);
            return response;
        }
    }
}