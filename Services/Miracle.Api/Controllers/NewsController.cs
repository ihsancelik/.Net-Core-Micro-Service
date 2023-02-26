using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Models.News;
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
    [Route(ControllerRoutes.News), ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService newsService;
        private readonly DataHelper dataHelper;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;

        public NewsController(INewsService newsService,
            DataHelper dataHelper,
            IMessageGeneratorService messageGeneratorService,
            ImageManagerService imageManagerService)
        {
            this.newsService = newsService;
            this.dataHelper = dataHelper;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.GetById)]
        public GetResponse<News> GetById(int id)
        {
            return newsService.GetResponse(id);
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.List)]
        public PagedListResponse<News> GetList(PaginationParameterModel model) //AdminPanel- News
        {
            if (ModelState.IsValid)
            {
                return newsService.GetPagedListResponse(model);
            }

            return new PagedListResponse<News>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Create)]
        public async Task<CreateResponse> CreateAsync([FromForm] NewsModel model)
        {
            if (model.NewsImage == null)
            {
                ModelState.AddModelError("Image", "Image cannot be empty");
            }

            if (ModelState.IsValid)
            {
                News news = new News();
                var result = dataHelper.FieldBinder(model, news);

                if (!result)
                    return new CreateResponse(dataHelper.Errors);


                news.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewsImage.FileName);

                var response = newsService.CreateResponse(news);

                if (response.Success)
                    await imageManagerService.SaveNewsImage(news.ImageName, model.NewsImage);

                return response;
            }

            return new CreateResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public async Task<EmptyResponse> UpdateAsync([FromRoute] int id, [FromForm] NewsModel model)
        {
            if (ModelState.IsValid)
            {
                var newsImage = model.NewsImage;

                var data = newsService.Get(id);
                if (data == null)
                {
                    var message = messageGeneratorService.PrepareResponseMessage("News", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);

                var oldImageName = data.ImageName;

                if (newsImage != null)
                {
                    data.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewsImage.FileName);
                }

                var response = newsService.UpdateResponse(data);

                if (response.Success && newsImage != null)
                {
                    await imageManagerService.UpdateNewsImage(oldImageName, data.ImageName, model.NewsImage);
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
                return newsService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return newsService.GetCountResponse();
        }

        #endregion

        [HttpGet, Route(NewsRoutes.GetImage)]
        public string GetNewsImage(int id)
        {
            return newsService.GetNewsImagePath(id).Data;
        }

        [HttpGet, Route(NewsRoutes.GetByTag)]
        public ListResponse<News> GetByTag(string tag)
        {
            return newsService.GetNewsByTag(tag);
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<News> GetListAll() // AdminPanel- Home
        {
            var newsList = newsService.GetList();
            var response = new ListResponse<News>();
            response.SetData(newsList);
            return response;
        }

        #region
        [HttpPost, AllowAnonymous, Route(NewsRoutes.NewsList)]
        public PagedListResponse<News> GetAll(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return newsService.GetPagedListResponse(model);
            }

            return new PagedListResponse<News>(this.GetModelStateErrors());
        }
        #endregion
    }
}