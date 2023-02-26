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
    public class PriorityService : IPriorityService
    {
        public ExceptionManager ExceptionManager { get; set; }

        private DatabaseResponse dbResponse;
        private readonly MainContext db;
        public PriorityService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public Priority Get(int id)
        {
            var data = db.Priorities.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Priority", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<Priority> GetList()
        {
            return db.Priorities;
        }
        public PagedResponse<Priority> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Priorities.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(Priority model)
        {
            var isExist = db.Priorities.Any(s => s.Name == model.Name || s.State == model.State);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Priority Name or State", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Priorities.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(Priority model)
        {
            var isExist = db.Priorities.Any(s => s.Id != model.Id && (s.Name == model.Name || s.State == model.State));
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Priority Name or State", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Priorities.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Priorities.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Priority", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Priorities.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.Priorities.Count();
        }
        #endregion

        #region Response
        public GetResponse<Priority> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<Priority>(ExceptionManager.Exceptions);

            var response = new GetResponse<Priority>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Priority> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<Priority>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<Priority> GetPagedListResponse(PaginationParameterModel model)
        {
            var data = GetPagedList(model);
            var response = new PagedListResponse<Priority>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Priority model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(Priority model)
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

        public GetResponse<Priority> GetResponseByVersion(int productId, int versionInfoId)
        {
            var data = db.ProductSettings
                .Include(s => s.Priority)
                .FirstOrDefault(s => s.ProductId == productId && s.VersionInfoId == versionInfoId)
                ?.Priority;

            if (data == null)
            {
                var message = MessageGenerator.Generate("Priority", MessageGeneratorActions.NotFound);
                return new GetResponse<Priority>(message);
            }

            var response = new GetResponse<Priority>();
            response.SetData(data);
            return response;
        }
    }
}