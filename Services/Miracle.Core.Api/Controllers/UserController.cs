using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.ProductLimitation;
using Miracle.Core.Api.Models.User;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.Services.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.User), ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly DataHelper dataHelper;
        private readonly ImageManagerService imageManagerService;

        private string[] contentTypes = new string[3] { "image/jpg", "image/jpeg", "image/png" };

        public UserController(IUserService userService, ImageManagerService imageManagerService)
        {
            this.userService = userService;
            dataHelper = new DataHelper();
            this.imageManagerService = imageManagerService;
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<User> GetById(int id)
        {
            var user = userService.Get(id);
            user.Password = string.Empty;
            var response = new GetResponse<User>();
            response.SetData(user);
            return response;

        }

        [HttpGet, MiracleAuthorize(Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<User> GetListAll()
        {
            var userList = userService.GetList();
            foreach (var user in userList)
            {
                user.Password = string.Empty;
            }
            var response = new ListResponse<User>();
            response.SetData(userList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<User> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                var response = userService.GetPagedListResponse(model);
                foreach (var user in response.PagedList.List)
                {
                    user.Password = string.Empty;
                }
                return response;
            }
            return new PagedListResponse<User>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(CRUDRoutes.Create)]
        public async Task<EmptyResponse> CreateAsync([FromForm] UserModel model)
        {
            if (model.ProfilePhoto != null && !contentTypes.Contains(model.ProfilePhoto.ContentType))
                ModelState.AddModelError("ProfilePhoto", "Allowed Extensions: Jpg, Jpeg and Png");

            if (model.ProfilePhoto?.Length > 2000000)
                ModelState.AddModelError("ProfilePhoto", "Image size must be smaller than 2MB");

            if (model.Username.Contains(" "))
                ModelState.AddModelError("Username", "Username cannot contains space");

            if (ModelState.IsValid)
            {
                User user = new User();
                var result = dataHelper.FieldBinder(model, user);

                if (!result)
                    return new EmptyResponse(dataHelper.Errors);


                foreach (int roleId in model.RoleIdList)
                {
                    user.User_Roles.Add(new User_Role
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    });
                }
                if (model.ProfilePhoto == null)
                    user.ImageName = "avatar.png";

                if (model.ProfilePhoto != null)
                    user.ImageName = Guid.NewGuid().ToString() + "." + model.ProfilePhoto.FileName.Substring(model.ProfilePhoto.FileName.IndexOf('.') + 1);

                var response = userService.CreateResponse(user);

                if (response.Success && model.ProfilePhoto != null)
                    await imageManagerService.SaveProfileImage(user.ImageName, model.ProfilePhoto);

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles.SD), Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromForm] UserUpdateModel model, [FromRoute] int id)
        {
            if (model.ProfilePhoto != null && !contentTypes.Contains(model.ProfilePhoto.ContentType))
                ModelState.AddModelError("ProfilePhoto", "Allowed Extensions: Jpg, Jpeg and Png");

            if (model.ProfilePhoto?.Length > 2000000)
                ModelState.AddModelError("ProfilePhoto", "Image size must be smaller than 2MB");

            if (model.Username.Contains(" "))
                ModelState.AddModelError("Username", "Username cannot contains space");

            if (ModelState.IsValid)
            {
                var data = userService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                data.User_Roles.Clear();
                foreach (var roleId in model.RoleIdList)
                {
                    var userRole = new User_Role()
                    {
                        UserId = data.Id,
                        RoleId = roleId
                    };
                    data.User_Roles.Add(userRole);
                }

                var oldImageName = "";
                if (model.ProfilePhoto != null)
                {
                    oldImageName = data.ImageName;
                    data.ImageName = Guid.NewGuid().ToString() + "." + model.ProfilePhoto.FileName.Substring(model.ProfilePhoto.FileName.IndexOf('.') + 1);
                }

                var response = userService.UpdateResponse(data);

                if (model.ProfilePhoto != null && response.Success)
                {
                    await imageManagerService.UpdateProfileImage(oldImageName, data.ImageName, model.ProfilePhoto);
                }

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var imageName = userService.Get(id).ImageName;
                var response = userService.DeleteResponse(id);
                if (response.Success)
                    imageManagerService.DeleteProfileImage(imageName);

                return response;
            }
            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return userService.GetCountResponse();
        }
        #endregion

        #region Product
        [HttpPost, MiracleAuthorize(Roles.SD, Roles.Admin), Route(UserRoutes.GetProducts)]
        public PagedListResponse<Product> GetProducts(int userId, PaginationParameterModel model)
        {
            return userService.GetProducts(userId, model);
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(UserRoutes.GetProductVersions)]
        public ListResponse<VersionInfo> GetProductVersions(int userId, int productId)
        {
            return userService.GetProductVersions(userId, productId);
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(UserRoutes.GetProductModules)]
        public PagedListResponse<User_Product_Module> GetProductModules(int userId, int productId, PaginationParameterModel model)
        {
            return userService.GetProductModules(userId, productId, model);
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(UserRoutes.GetProductLimitation)]
        public GetResponse<ProductLimitation> GetProductLimitation(int userId, int productId)
        {
            return userService.GetProductLimitation(userId, productId);
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(UserRoutes.AddProduct)]
        public EmptyResponse AddProduct(int userId, int productId, [FromBody] ProductLimitationModel model)
        {
            var productLimitation = new ProductLimitation();

            return dataHelper.FieldBinder(model, productLimitation) ?
                userService.AddProduct(userId, productId, productLimitation) :
            new EmptyResponse(dataHelper.Errors);
        }

        [HttpDelete, MiracleAuthorize(Roles.SD), Route(UserRoutes.RemoveProduct)]
        public EmptyResponse RemoveProduct(int userId, int productId)
        {
            return userService.RemoveProduct(userId, productId);
        }

        [HttpPost, MiracleAuthorize(Roles.SD), Route(UserRoutes.AddVersion)]
        public EmptyResponse AddVersion(int userId, int productId, [FromBody] UserProductVersionModel model)
        {
            return userService.AddVersion(userId, productId, model.VersionInfoIdList);
        }

        [HttpGet, MiracleAuthorize(Roles.SD), Route(UserRoutes.AddModule)]
        public EmptyResponse AddModule(int userId, int productId, int moduleId, bool isActive)
        {
            return userService.AddModule(userId, productId, moduleId, isActive);
        }

        [HttpDelete, MiracleAuthorize(Roles.SD), Route(UserRoutes.RemoveModule)]
        public EmptyResponse RemoveModule(int userId, int productId, int moduleId)
        {
            return userService.RemoveModule(userId, productId, moduleId);
        }
        #endregion

        [HttpGet, Route(UserRoutes.GetProfileImage)]
        public string GetProfileImage(int id)
        {
            var imageName = userService.Get(id)?.ImageName;
            var imagePath = imageManagerService.GetProfileImage(imageName);

            return imagePath;
        }
    }
}