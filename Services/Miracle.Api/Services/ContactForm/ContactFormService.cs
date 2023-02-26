using Miracle.Api.Database;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Repositories;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System;
using System.Linq;

namespace Miracle.Api.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly IBaseRepository<MainContext, ContactForm> contactFormRepository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public ContactFormService(IBaseRepository<MainContext, ContactForm> contactFormRepository, IMessageGeneratorService messageGeneratorService)
        {
            this.contactFormRepository = contactFormRepository;
            this.messageGeneratorService = messageGeneratorService;
        }

        #region Common
        public PagedListResponse<ContactForm> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<ContactForm>();
            var list = contactFormRepository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public ContactForm Get(int id)
        {
            return contactFormRepository.Get().FirstOrDefault(c => c.Id == id);
        }
        public GetResponse<ContactForm> GetResponse(int id)
        {
            var data = contactFormRepository.Get().FirstOrDefault(c => c.Id == id);
            var response = new GetResponse<ContactForm>();
            response.SetData(data);

            return response;
        }
        public IQueryable<ContactForm> GetList()
        {
            return contactFormRepository.Table;
        }
        public ListResponse<ContactForm> GetListResponse()
        {
            var list = contactFormRepository.Table.ToList();
            var response = new ListResponse<ContactForm>();
            response.SetData(list);
            return response;
        }
        public CreateResponse CreateResponse(ContactForm value)
        {
            contactFormRepository.Create(value);

            var dbResult = contactFormRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);
            return response;
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = contactFormRepository.Get().FirstOrDefault(c => c.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("ContactForm", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            contactFormRepository.Table.Remove(data);
            var dbResult = contactFormRepository.Save();

            return new EmptyResponse(dbResult);
        }

        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(contactFormRepository.Table.Count());
            return response;
        }

        #endregion

        #region UnUsed
        public EmptyResponse UpdateResponse(ContactForm value)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
