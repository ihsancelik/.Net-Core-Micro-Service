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
    public class SMTPSettingService : ISMTPSettingService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;

        public ExceptionManager ExceptionManager { get; set; }
        public SMTPSettingService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public SMTPSetting Get(int id)
        {
            var data = db.SMTPSettings.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("SMTPSetting", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<SMTPSetting> GetList()
        {
            return db.SMTPSettings;
        }
        public PagedResponse<SMTPSetting> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.SMTPSettings.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(SMTPSetting model)
        {
            var isExist = db.SMTPSettings.Any(s => s.Email == model.Email);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("SMTPSetting", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.SMTPSettings.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(SMTPSetting model)
        {
            var isExist = db.SMTPSettings.Any(s => s.Id != model.Id && s.Email == model.Email);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("SMTPSetting", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.SMTPSettings.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.SMTPSettings.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("SMTPSetting", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.SMTPSettings.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.SMTPSettings.Count();
        }
        #endregion

        #region Response
        public GetResponse<SMTPSetting> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<SMTPSetting>(ExceptionManager.Exceptions);

            var response = new GetResponse<SMTPSetting>();
            response.SetData(data);
            return response;
        }
        public ListResponse<SMTPSetting> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<SMTPSetting>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<SMTPSetting> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<SMTPSetting>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(SMTPSetting model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(SMTPSetting model)
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