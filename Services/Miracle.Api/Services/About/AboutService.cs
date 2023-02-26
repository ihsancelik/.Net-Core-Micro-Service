using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System;
using System.Linq;

namespace Miracle.Api.Services
{
    public class AboutService : IAboutService
    {
        private readonly IBaseRepository<MainContext, About> aboutRepository;
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly ImageManagerService imageManagerService;

        public AboutService(IBaseRepository<MainContext, About> aboutRepository,
            IMessageGeneratorService messageGeneratorService,
            ImageManagerService imageManagerService)
        {
            this.aboutRepository = aboutRepository;
            this.messageGeneratorService = messageGeneratorService;
            this.imageManagerService = imageManagerService;
        }

        public About Get()
        {
            return aboutRepository.Get().FirstOrDefault();
        }
        public About Get(int id)
        {
            return aboutRepository.Get(s => s.Id == id);
        }
        public GetResponse<About> GetResponse(int id)
        {
            var data = aboutRepository.Get().FirstOrDefault(a => a.Id == id);

            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("About", MessageGeneratorActions.NotFound);
                return new GetResponse<About>(message);
            }

            var response = new GetResponse<About>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse UpdateResponse(About value)
        {
            int count = aboutRepository.Table.Count();

            if (count == 0)
                aboutRepository.Table.Add(value);
            else
                aboutRepository.Table.Update(value);

            var dbResult = aboutRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public GetResponse<string> GetAboutImagePath()
        {
            var imageName = aboutRepository.Get().FirstOrDefault().ImageName;
            var imagePath = imageManagerService.GetAboutImage(imageName);
            var response = new GetResponse<string>();
            response.SetData(imagePath);
            return response;
        }

        #region UnUsed
        public IQueryable<About> GetList()
        {
            throw new NotImplementedException();
        }
        public ListResponse<About> GetListResponse()
        {
            throw new NotImplementedException();
        }
        public PagedListResponse<About> GetPagedListResponse(PaginationParameterModel model)
        {
            throw new NotImplementedException();
        }
        public CreateResponse CreateResponse(About value)
        {
            throw new NotImplementedException();
        }
        public EmptyResponse DeleteResponse(int id)
        {
            throw new NotImplementedException();
        }
        public GetResponse<object> GetCountResponse()
        {
            throw new NotImplementedException();
        }
        #endregion

        public About GetFirst()
        {
            return aboutRepository.Get().FirstOrDefault();
        }

    }
}