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
    public class VersionInfoService : IVersionInfoService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;

        public ExceptionManager ExceptionManager { get; set; }
        public VersionInfoService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public VersionInfo Get(int id)
        {
            var data = db.VersionInfos.FirstOrDefault(s => s.Id == id);
            if (data == null)
                ExceptionManager.AddException(MessageGenerator.Generate("VersionInfo", MessageGeneratorActions.NotFound));

            return data;
        }
        public IQueryable<VersionInfo> GetList()
        {
            return db.VersionInfos;
        }
        public PagedResponse<VersionInfo> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.VersionInfos.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(VersionInfo model)
        {
            var isExist = db.VersionInfos.Any(s => s.Version == model.Version);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("VersionInfo", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.VersionInfos.Add(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(VersionInfo model)
        {
            var isExist = db.VersionInfos.Any(s => s.Id != model.Id && s.Version == model.Version);
            if (isExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("VersionInfo", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.VersionInfos.Update(model);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.VersionInfos.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("VersionInfo", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.VersionInfos.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.VersionInfos.Count();
        }
        #endregion

        #region Response
        public GetResponse<VersionInfo> GetResponse(int id)
        {
            var data = Get(id);
            if (ExceptionManager.HaveException)
                return new GetResponse<VersionInfo>(ExceptionManager.Exceptions);

            var response = new GetResponse<VersionInfo>();
            response.SetData(data);
            return response;
        }
        public ListResponse<VersionInfo> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<VersionInfo>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<VersionInfo> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<VersionInfo>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(VersionInfo model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(VersionInfo model)
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

        public PagedListResponse<VersionInfo> GetListByProductResponse(int productId, PaginationParameterModel paginationModel)
        {
            var response = new PagedListResponse<VersionInfo>();

            var data = db.ProductSettings
                .Where(s => s.ProductId == productId)
                .Include(s => s.VersionInfo)
                .Select(s => s.VersionInfo)
                .GetPaged(paginationModel);

            response.SetData(data);
            return response;
        }

        public ListResponse<VersionInfo> GetListByUserProduct(int productId)
        {
            var response = new ListResponse<VersionInfo>();

            var data = db.ProductSettings
                .Where(s => s.ProductId == productId)
                .Include(s => s.VersionInfo)
                .Select(s => s.VersionInfo);

            response.SetData(data);
            return response;
        }
    }
}