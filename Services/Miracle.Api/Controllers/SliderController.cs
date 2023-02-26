using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Models.Slider;
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
    [Route(ControllerRoutes.Slider), ApiController]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService sliderService;
        private readonly DataHelper dataHelper;
        private readonly ImageManagerService imageManagerService;
        private readonly IMessageGeneratorService messageGeneratorService;

        public SliderController(ISliderService sliderService, DataHelper dataHelper, ImageManagerService imageManagerService, IMessageGeneratorService messageGeneratorService)
        {
            this.sliderService = sliderService;
            this.dataHelper = dataHelper;
            this.imageManagerService = imageManagerService;
            this.messageGeneratorService = messageGeneratorService;
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.GetById)]
        public GetResponse<Slider> GetById(int id)
        {
            return sliderService.GetResponse(id);
        }

        [HttpGet, Route(SliderRoutes.GetImage)]
        public string GetSliderImage(int id)
        {
            return sliderService.GetSliderImagePath(id).Data;
        }

        [HttpPost, Route(CRUDRoutes.List)]
        public PagedListResponse<Slider> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return sliderService.GetPagedListResponse(model);
            }

            return new PagedListResponse<Slider>(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<Slider> GetListAll()
        {
            var sliderList = sliderService.GetList();
            var response = new ListResponse<Slider>();
            response.SetData(sliderList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Create)]
        public async Task<CreateResponse> CreateAsync([FromForm] SliderModel model)
        {
            if (model.SliderImage == null)
            {
                ModelState.AddModelError("Image", "Image cannot be empty");
            }

            if (ModelState.IsValid)
            {
                Slider slider = new Slider();
                var result = dataHelper.FieldBinder(model, slider);

                if (!result)
                    return new CreateResponse(dataHelper.Errors);


                slider.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.SliderImage.FileName);

                var response = sliderService.CreateResponse(slider);

                if (response.Success)
                    await imageManagerService.SaveSliderImage(slider.ImageName, model.SliderImage);

                return response;
            }

            return new CreateResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromRoute] int id, [FromForm] SliderModel model)
        {
            if (ModelState.IsValid)
            {
                var sliderImage = model.SliderImage;

                var data = sliderService.Get(id);
                if (data == null)
                {
                    var message = messageGeneratorService.PrepareResponseMessage("Slider", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var oldImageName = data.ImageName;

                if (sliderImage != null)
                {
                    data.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.SliderImage.FileName);
                }

                var response = sliderService.UpdateResponse(data);

                if (response.Success && sliderImage != null)
                {
                    await imageManagerService.UpdateSliderImage(oldImageName, data.ImageName, model.SliderImage);
                }

                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return sliderService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return sliderService.GetCountResponse();
        }
        #endregion
    }
}