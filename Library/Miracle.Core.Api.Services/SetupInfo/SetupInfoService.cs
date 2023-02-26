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
    public class SetupInfoService : ISetupInfoService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;

        public ExceptionManager ExceptionManager { get; set; }
        public SetupInfoService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public SetupInfo Get(int id)
        {
            return db.SetupInfos.FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<SetupInfo> GetList()
        {
            return db.SetupInfos;
        }
        public PagedResponse<SetupInfo> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.SetupInfos.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(SetupInfo model)
        {
            db.SetupInfos.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(SetupInfo model)
        {
            db.SetupInfos.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.SetupInfos.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("SetupInfo", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.SetupInfos.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.SetupInfos.Count();
        }
        #endregion

        #region Response
        public GetResponse<SetupInfo> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<SetupInfo>(ExceptionManager.Exceptions);

            var response = new GetResponse<SetupInfo>();
            response.SetData(data);
            return response;
        }
        public ListResponse<SetupInfo> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<SetupInfo>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<SetupInfo> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<SetupInfo>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(SetupInfo model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(SetupInfo model)
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