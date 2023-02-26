using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public class AppLibService : IAppLibService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }

        public AppLibService(MainContext db)
        {
            this.db = db;
            ExceptionManager = new ExceptionManager();
            dbResponse = new DatabaseResponse();
        }

        #region Common
        public AppLib Get(int id)
        {
            var data = db.AppLibs.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Notice", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<AppLib> GetList()
        {
            return db.AppLibs;
        }
        public List<AppLib> GetList(bool isActive)
        {
            return db.AppLibs.Where(s => s.IsActive == isActive).ToList();
        }
        public PagedResponse<AppLib> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.AppLibs.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(AppLib model)
        {
            var isExist = db.AppLibs.Any(s => s.LibName == model.LibName);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("AppLib", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.AppLibs.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(AppLib model)
        {
            var isExist = db.AppLibs.Any(s => s.Id != model.Id && s.LibName == model.LibName);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("AppLib", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.AppLibs.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.AppLibs.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("AppLib", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.AppLibs.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.AppLibs.Count();
        }
        #endregion

        #region Response
        public GetResponse<AppLib> GetResponse(int id)
        {
            var data = Get(id);

            if (ExceptionManager.HaveException)
                return new GetResponse<AppLib>(ExceptionManager.Exceptions);

            var response = new GetResponse<AppLib>();
            response.SetData(data);
            return response;
        }
        public ListResponse<AppLib> GetListResponse()
        {
            var list = GetList();
            var response = new ListResponse<AppLib>();
            response.SetData(list);
            return response;
        }
        public PagedListResponse<AppLib> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var list = GetPagedList(paginationModel);
            var response = new PagedListResponse<AppLib>();
            response.SetData(list);
            return response;
        }
        public EmptyResponse CreateResponse(AppLib model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(AppLib model)
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
