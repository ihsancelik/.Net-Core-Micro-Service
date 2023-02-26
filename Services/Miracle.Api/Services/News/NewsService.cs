using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Linq;

namespace Miracle.Api.Services
{
    public class NewsService : INewsService
    {
        private readonly IBaseRepository<MainContext, News> newsRepository;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;

        public NewsService(IBaseRepository<MainContext, News> newsRepository,
            IMessageGeneratorService messageGeneratorService,
            ImageManagerService imageManagerService)
        {
            this.newsRepository = newsRepository;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
        }
        #region Common

        public PagedListResponse<News> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<News>();
            var list = newsRepository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public News Get(int id)
        {
            return newsRepository.Get().FirstOrDefault(n => n.Id == id);
        }
        public GetResponse<News> GetResponse(int id)
        {
            var data = newsRepository.Get().FirstOrDefault(n => n.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("News", MessageGeneratorActions.NotFound);
                return new GetResponse<News>(message);
            }

            var response = new GetResponse<News>();
            response.SetData(data);
            return response;
        }
        public IQueryable<News> GetList()
        {
            return newsRepository.Table.Where(n => n.IsActive);
        }
        public ListResponse<News> GetListResponse()
        {
            var list = newsRepository.Table.ToList();
            var response = new ListResponse<News>();
            response.SetData(list);
            return response;
        }
        public CreateResponse CreateResponse(News value)
        {
            newsRepository.Create(value);

            var dbResult = newsRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);
            return response;
        }
        public EmptyResponse UpdateResponse(News value)
        {
            newsRepository.Table.Update(value);
            var dbResult = newsRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = newsRepository.Get().FirstOrDefault(n => n.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("News", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            newsRepository.Table.Remove(data);
            var dbResult = newsRepository.Save();

            var response = new EmptyResponse(dbResult);

            if (response.Success)
                imageManagerService.DeleteNewsImage(data.ImageName);

            return response;
        }
        #endregion

        public GetResponse<string> GetNewsImagePath(int id)
        {
            var imageName = newsRepository.Get().FirstOrDefault(n => n.Id == id).ImageName;
            var imagePath = imageManagerService.GetNewsImage(imageName);
            var response = new GetResponse<string>();
            response.SetData(imagePath);
            return response;
        }
        public ListResponse<News> GetNewsByTag(string tags)
        {
            var list = newsRepository.Get().Where(n => n.Tags == tags).ToList();
            var response = new ListResponse<News>();
            response.SetData(list);
            return response;
        }

        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(newsRepository.Table.Count());
            return response;
        }
    }
}