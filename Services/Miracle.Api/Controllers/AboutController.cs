using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models.About;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.About), ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService aboutService;
        private readonly DataHelper dataHelper;
        private readonly ImageManagerService imageManagerService;

        public AboutController(IAboutService aboutService,
            DataHelper dataHelper,
            ImageManagerService imageManagerService)
        {
            this.aboutService = aboutService;
            this.dataHelper = dataHelper;
            this.imageManagerService = imageManagerService;
        }

        #region Admin Panel

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromForm] AboutModel model)
        {
            if (ModelState.IsValid)
            {
                var aboutImage = model.AboutImage;
                var data = aboutService.GetFirst();

                if (data == null)
                    data = new About();

                var result = dataHelper.FieldBinder(model, data);

                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var oldImageName = data.ImageName;
                if (aboutImage != null)
                {
                    data.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.AboutImage.FileName);
                }

                var response = aboutService.UpdateResponse(data);

                if (response.Success && aboutImage != null)
                {
                    await imageManagerService.UpdateAboutImage(oldImageName, data.ImageName, model.AboutImage);
                }

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }
        #endregion

        #region Miracle
        [HttpGet, Route(AboutRoutes.Get)]
        public GetResponse<About> Get()
        {
            var data = aboutService.GetFirst();

            var response = new GetResponse<About>();
            response.SetData(data);

            return response;
        }

        [HttpGet, Route(AboutRoutes.GetImage)]
        public string GetAboutImage()
        {
            return aboutService.GetAboutImagePath().Data;
        }
        #endregion
    }
}