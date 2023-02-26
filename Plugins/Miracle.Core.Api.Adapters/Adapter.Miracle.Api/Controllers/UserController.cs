using Adapter.Miracle.Api.Models;
using Adapter.Miracle.Api.Services;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Adapter.Miracle.Api.Controllers
{
    [Route(ControllerRoutes.User), ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserServiceAdapter userServiceAdapter;
        private readonly ImageManagerService imageManagerService;
        private readonly DataHelper dataHelper;


        public UserController(IUserService userService, IUserServiceAdapter userServiceAdapter, ImageManagerService imageManagerService)
        {
            this.userService = userService;
            this.userServiceAdapter = userServiceAdapter;
            this.imageManagerService = imageManagerService;
            dataHelper = new DataHelper();
        }

        [HttpGet, MiracleAuthorize(Roles.User), Route(UserRoutes.GetOutSource)]
        public GetResponse<User> GetOutSource()
        {
            if (ModelState.IsValid)
            {
                var userId = this.GetId();

                var user = userService.Get(userId);
                if (user == null)
                {
                    var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                    return new GetResponse<User>(message);
                }

                var response = new GetResponse<User>();
                response.SetData(user);
                return response;
            }
            return new GetResponse<User>(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles.User), Route(UserRoutes.UpdateOutSource)]
        public async Task<EmptyResponse> UpdateOutSourceAsync([FromForm] UserOutSourceModel model)
        {
            var userId = this.GetId();

            if (model.Username.Contains(" "))
                ModelState.AddModelError("Username", "Username cannot contains space");

            if (ModelState.IsValid)
            {
                var data = userService.Get(userId);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data, new List<string>() { "Id" });
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

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

        [HttpGet, MiracleAuthorize, Route(UserRoutes.GetProfileImageOutSource)]
        public GetResponse<string> GetProfileImageOutSource()
        {
            if (ModelState.IsValid)
            {
                var id = this.GetId();
                var imageName = userService.Get(id)?.ImageName;
                var imagePath = imageManagerService.GetProfileImage(imageName);

                var response = new GetResponse<string>();
                response.SetData(imagePath);
                return response;
            }
            return new GetResponse<string>(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize, Route(UserRoutes.AddProductOutSource)]
        public EmptyResponse AddProductOutSource([FromRoute] int userId, [FromRoute] string tag, [FromRoute] int versionId)
        {
            if (ModelState.IsValid)
                return userServiceAdapter.AddProductOutSource(userId, tag, versionId);
            return new EmptyResponse(this.GetModelStateErrors());
        }
    }
}