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
    public class SliderService : ISliderService
    {
        private readonly IBaseRepository<MainContext, Slider> sliderRepository;
        private readonly ImageManagerService imageManagerService;
        private readonly IMessageGeneratorService messageGeneratorService;

        public SliderService(IBaseRepository<MainContext, Slider> sliderRepository, ImageManagerService imageManagerService, IMessageGeneratorService messageGeneratorService)
        {
            this.sliderRepository = sliderRepository;
            this.imageManagerService = imageManagerService;
            this.messageGeneratorService = messageGeneratorService;
        }

        public PagedListResponse<Slider> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<Slider>();
            var list = sliderRepository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public Slider Get(int id)
        {
            return sliderRepository.Get().FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<Slider> GetList()
        {
            return sliderRepository.Table.Where(s => s.IsActive);
        }
        public ListResponse<Slider> GetListResponse()
        {
            var list = sliderRepository.Table.ToList();
            var response = new ListResponse<Slider>();
            response.SetData(list);
            return response;
        }
        public GetResponse<Slider> GetResponse(int id)
        {
            var data = sliderRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Slider", MessageGeneratorActions.NotFound);
                return new GetResponse<Slider>(message);
            }

            var response = new GetResponse<Slider>();
            response.SetData(data);
            return response;
        }
        public CreateResponse CreateResponse(Slider value)
        {
            sliderRepository.Create(value);

            var dbResult = sliderRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);
            return response;
        }
        public EmptyResponse UpdateResponse(Slider value)
        {
            sliderRepository.Table.Update(value);
            var dbResult = sliderRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = sliderRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Slider", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            sliderRepository.Table.Remove(data);
            var dbResult = sliderRepository.Save();

            var response = new EmptyResponse(dbResult);

            if (response.Success)
                imageManagerService.DeleteSliderImage(data.ImageName);

            return response;
        }
        public GetResponse<string> GetSliderImagePath(int id)
        {
            var imageName = sliderRepository.Get().FirstOrDefault(s => s.Id == id).ImageName;
            var imagePath = imageManagerService.GetSliderImage(imageName);
            var response = new GetResponse<string>();
            response.SetData(imagePath);
            return response;
        }
        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(sliderRepository.Table.Count());
            return response;
        }
    }
}
