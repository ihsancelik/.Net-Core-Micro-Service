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
    public class NoticeService : INoticeService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }
        public NoticeService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public Notice Get(int id)
        {
            var data = db.Notices.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Notice", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<Notice> GetList()
        {
            return db.Notices;
        }
        public PagedResponse<Notice> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Notices.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(Notice model)
        {
            db.Notices.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
            {
                ExceptionManager.AddException(dbResponse.Exception);
            }
            return dbResponse;
        }
        public DatabaseResponse Update(Notice model)
        {
            db.Notices.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
            {
                ExceptionManager.AddException(dbResponse.Exception);
            }
            return dbResponse;

        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Notices.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Notice", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Notices.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
            {
                ExceptionManager.AddException(dbResponse.Exception);
            }
            return dbResponse;

        }
        public int Count()
        {
            return db.Notices.Count();
        }
        #endregion

        #region Response
        public GetResponse<Notice> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<Notice>(ExceptionManager.Exceptions);

            var response = new GetResponse<Notice>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Notice> GetListResponse()
        {
            var list = db.Notices.ToList();
            var response = new ListResponse<Notice>();
            response.SetData(list);
            return response;
        }
        public PagedListResponse<Notice> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<Notice>();
            var data = GetPagedList(model);
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Notice value)
        {
            Create(value);
            if (ExceptionManager.HaveException)
            {
                return new EmptyResponse(ExceptionManager.Exceptions);
            }
            return new EmptyResponse();
        }
        public EmptyResponse UpdateResponse(Notice model)
        {
            Update(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse();
        }
        public EmptyResponse DeleteResponse(int id)
        {
            Delete(id);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse();
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