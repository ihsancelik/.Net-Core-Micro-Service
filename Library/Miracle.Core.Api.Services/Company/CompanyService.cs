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
using System.Linq.Dynamic.Core;

namespace Miracle.Core.Api.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }
        public CompanyService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public Company Get(int id)
        {
            var data = db.Companies.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("Company", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<Company> GetList()
        {
            return db.Companies;
        }
        public PagedResponse<Company> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Companies.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(Company model)
        {
            var isExist = db.Companies.Any(s => s.Name == model.Name);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Company", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Companies.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(Company model)
        {
            var isExist = db.Companies.Any(s => s.Id != model.Id && s.Name == model.Name);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Company", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Companies.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Companies.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Company", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Companies.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.Companies.Count();
        }
        #endregion

        #region Response
        public GetResponse<Company> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<Company>(ExceptionManager.Exceptions);

            var response = new GetResponse<Company>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Company> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<Company>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<Company> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<Company>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Company model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(Company model)
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