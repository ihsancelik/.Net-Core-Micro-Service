using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Api.Services
{
    public class FeedbackService : IFeedBackService
    {
        private readonly IBaseRepository<MainContext, FeedBack> feedBackRespository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public FeedbackService(IBaseRepository<MainContext, FeedBack> feedBackRespository, 
            IMessageGeneratorService messageGeneratorService)
        {
            this.feedBackRespository = feedBackRespository;
            this.messageGeneratorService = messageGeneratorService;
        } 
        
        #region Common
        public PagedListResponse<FeedBack> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<FeedBack>();
            var list = feedBackRespository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public FeedBack Get(int id)
        {
            return feedBackRespository.Get().FirstOrDefault(s => s.Id == id);
        }
        public GetResponse<FeedBack> GetResponse(int id)
        {
            var data = feedBackRespository.Get().FirstOrDefault(s => s.Id == id);
            var response = new GetResponse<FeedBack>();
            response.SetData(data);
            return response;
        }
        public IQueryable<FeedBack> GetList()
        {
            return feedBackRespository.Table;
        }
        public ListResponse<FeedBack> GetListResponse()
        {
            var list = feedBackRespository.Table.ToList();
            var response = new ListResponse<FeedBack>();
            response.SetData(list);
            return response;
        }
        public CreateResponse CreateResponse(FeedBack value)
        {
            value.CreatedDate = DateTime.Now;

            feedBackRespository.Create(value);

            var dbResult = feedBackRespository.Save();
            var response = new CreateResponse(dbResult);

            //response.SetData(value.Id);
            return response;
        }
        public EmptyResponse UpdateResponse(FeedBack value)
        {
            throw new System.NotImplementedException();
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = feedBackRespository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("FeedBack", Enums.MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            feedBackRespository.Table.Remove(data);
            var dbResult = feedBackRespository.Save();
            return new EmptyResponse(dbResult);
        }
        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(feedBackRespository.Table.Count());
            return response;
        }
        #endregion

        public List<string> GetOptions()
        {
            List<string> options = new List<string>();
            options.Add("Öneri");
            options.Add("Sorun Bildir");

            return new List<string>(options);
        }
    }
}