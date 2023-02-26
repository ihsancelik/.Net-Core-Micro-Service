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
    public class RoleService : IRoleService
    {
        private DatabaseResponse dbResponse;
        private readonly MainContext db;

        public ExceptionManager ExceptionManager { get; set; }
        public RoleService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();

            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public Role Get(int id)
        {
            return db.Roles.FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<Role> GetList()
        {
            return db.Roles
                .Include(r => r.User_Roles);
        }
        public PagedResponse<Role> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Roles.Include(r => r.User_Roles).GetPaged(paginationModel);
        }
        public DatabaseResponse Create(Role model)
        {
            var isExist = db.Roles.Any(s => s.Value == model.Value);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Role Value", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Roles.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(Role model)
        {
            var isExist = db.Roles.Any(s => s.Id != model.Id && s.Value == model.Value);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Role Value", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Roles.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Roles.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Role", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Roles.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.Roles.Count();
        }
        #endregion

        #region Response
        public GetResponse<Role> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<Role>(ExceptionManager.Exceptions);

            var response = new GetResponse<Role>();
            response.SetData(data);
            return response;
        }
        public ListResponse<Role> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<Role>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<Role> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<Role>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(Role model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(Role model)
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