using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Helpers.Security;
using Library.Responses.Common;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public class UserService : IUserService
    {
        private readonly MainContext db;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }
        public UserService(MainContext db)
        {
            this.db = db;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }

        #region Common
        public User Get(int userId)
        {
            return db.Users
                .Include(s => s.User_Roles)
                .Include(s => s.User_Products)
                .FirstOrDefault(s => s.Id == userId);
        }
        public IQueryable<User> GetList()
        {
            return db.Users
                .Include(s => s.User_Products)
                .Include(s => s.User_Roles);
        }
        public PagedResponse<User> GetPagedList(PaginationParameterModel paginationModel)
        {
            return db.Users.GetPaged(paginationModel);
        }
        public DatabaseResponse Create(User model)
        {
            model.Username = model.Username.ToLower();
            model.Password = SHA512Encryptor.Encrypt(model.Password);

            var exist = db.Users.Any(s => s.Email == model.Email);
            if (exist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("User Email", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            exist = db.Users.Any(s => s.Username == model.Username);
            if (exist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Username", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Users.Add(model);
            dbResponse = db.Save();
            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Update(User model)
        {
            model.Username = model.Username.ToLower().Replace(" ", string.Empty);

            var emailExist = db.Users.Any(s => s.Id != model.Id && s.Email == model.Email);
            if (emailExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("User Email", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            var usernameExist = db.Users.Any(s => s.Id != model.Id && s.Username == model.Username);
            if (usernameExist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Username", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Entry(model).State = EntityState.Modified;
            db.Users.Update(model);

            dbResponse = db.Save();
            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public DatabaseResponse Delete(int id)
        {
            var data = db.Users.FirstOrDefault(u => u.Id == id);

            if (data == null)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("User", MessageGeneratorActions.NotFound));
                return dbResponse;
            }

            db.Users.Remove(data);
            dbResponse = db.Save();

            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public int Count()
        {
            return db.Users.Count();
        }
        #endregion

        #region Common Response
        public GetResponse<User> GetResponse(int userId)
        {
            var data = Get(userId);
            if (ExceptionManager.HaveException)
                return new GetResponse<User>(ExceptionManager.Exceptions);

            var response = new GetResponse<User>();
            response.SetData(data);
            return response;
        }
        public ListResponse<User> GetListResponse()
        {
            var data = GetList();
            var response = new ListResponse<User>();
            response.SetData(data);
            return response;
        }
        public PagedListResponse<User> GetPagedListResponse(PaginationParameterModel paginationModel)
        {
            var data = GetPagedList(paginationModel);
            var response = new PagedListResponse<User>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse CreateResponse(User model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
        public EmptyResponse UpdateResponse(User model)
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
            var count = Count();
            var response = new GetResponse<object>();
            response.SetData(count);
            return response;
        }
        #endregion

        #region Product Response
        public PagedListResponse<Product> GetProducts(int userId, PaginationParameterModel model)
        {
            var products = db.User_Products
                .Where(s => s.UserId == userId)
                .Include(s => s.Product)
                .Select(s => s.Product)
                .GetPaged(model);

            var response = new PagedListResponse<Product>();
            response.SetData(products);
            return response;
        }
        public ListResponse<VersionInfo> GetProductVersions(int userId, int productId)
        {
            var versions = db.User_Versions
                .Where(s => s.UserId == userId && s.ProductId == productId)
                .Include(s => s.VersionInfo)
                .Select(s => s.VersionInfo)
                .ToList();

            var response = new ListResponse<VersionInfo>();
            response.SetData(versions);
            return response;
        }
        public PagedListResponse<User_Product_Module> GetProductModules(int userId, int productId, PaginationParameterModel model)
        {
            var productModules = db.User_Product_Modules
                 .Where(s => s.UserId == userId && s.ProductId == productId)
                 .Include(s => s.Module)
                 .GetPaged(model);

            var response = new PagedListResponse<User_Product_Module>();
            response.SetData(productModules);
            return response;
        }
        public GetResponse<ProductLimitation> GetProductLimitation(int userId, int productId)
        {
            var productLimitation = db.User_Products
               .Where(s => s.UserId == userId && s.ProductId == productId)
               .Include(s => s.ProductLimitation)
               .Select(s => s.ProductLimitation)
               .FirstOrDefault();

            var response = new GetResponse<ProductLimitation>();
            response.SetData(productLimitation);
            return response;
        }
        public EmptyResponse AddProduct(int userId, int productId, ProductLimitation productLimitation)
        {
            var user_Product = db.User_Products.FirstOrDefault(s => s.UserId == userId && s.ProductId == productId);

            // Create a new
            if (user_Product == null)
            {
                db.ProductLimitations.Add(productLimitation);
                db.Save();

                db.User_Products.Add(new User_Product()
                {
                    UserId = userId,
                    ProductId = productId,
                    ProductLimitationId = productLimitation.Id
                });
            }
            // Update
            else
            {
                int productLimitationId = user_Product.ProductLimitationId;
                var pl = db.ProductLimitations.FirstOrDefault(s => s.Id == productLimitationId);
                pl.IsActive = productLimitation.IsActive;
                pl.IsDemo = productLimitation.IsDemo;
                pl.DemoStartDate = productLimitation.DemoStartDate;
                pl.DemoEndDate = productLimitation.DemoEndDate;

                if (!productLimitation.IsDemo)
                {
                    pl.DemoStartDate = null;
                    pl.DemoEndDate = null;
                }
                db.ProductLimitations.Update(pl);
                db.User_Products.Update(user_Product);
            }

            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
        public EmptyResponse RemoveProduct(int userId, int productId)
        {
            var userProduct = db.User_Products
                .FirstOrDefault(s => s.UserId == userId && s.ProductId == productId);
            db.User_Products.Remove(userProduct);
            var dbResult = db.Save();

            var userVersions = db.User_Versions
                .Where(s => s.UserId == userId && s.ProductId == productId)
                .ToList();

            db.User_Versions.RemoveRange(userVersions);
            dbResult.StateEntriesCount += db.SaveChanges();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse AddVersion(int userId, int productId, int[] versionInfoIdList)
        {
            var user = db.Users.Include(s => s.User_Versions).FirstOrDefault(s => s.Id == userId);

            if (user == null)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            var currentUserVersions = new List<User_Version>(user.User_Versions.Where(s => s.ProductId == productId).ToList());
            foreach (var currentUserVersion in currentUserVersions)
                user.User_Versions.Remove(currentUserVersion);

            foreach (var versionInfoId in versionInfoIdList)
            {
                user.User_Versions.Add(new User_Version()
                {
                    UserId = userId,
                    ProductId = productId,
                    VersionInfoId = versionInfoId
                });
            }
            db.Users.Update(user);
            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
        public EmptyResponse AddModule(int userId, int productId, int moduleId, bool isActive)
        {
            var user = db.Users.Include(s => s.User_Product_Modules).FirstOrDefault(s => s.Id == userId);
            var user_Product_Module = user.User_Product_Modules.FirstOrDefault(s => s.UserId == userId && s.ProductId == productId && s.ProductModuleId == moduleId);

            if (user == null)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            if (user_Product_Module == null)
            {
                user.User_Product_Modules.Add(new User_Product_Module()
                {
                    UserId = userId,
                    ProductId = productId,
                    ProductModuleId = moduleId,
                    IsActive = isActive
                });
            }
            else
            {
                user_Product_Module.IsActive = isActive;
            }

            db.Users.Update(user);
            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
        public EmptyResponse RemoveModule(int userId, int productId, int moduleId)
        {
            var userProductModel = db.User_Product_Modules
                .FirstOrDefault(s => s.UserId == userId && s.ProductId == productId && s.ProductModuleId == moduleId);
            db.User_Product_Modules.Remove(userProductModel);
            var dbResult = db.Save();

            return new EmptyResponse(dbResult);
        }
        #endregion

        #region Unused
     
        #endregion
    }
}