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
    public class ContactInfoService : IContactInfoService
    {
        private readonly IBaseRepository<MainContext, ContactInfo> contactInfoRepository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public ContactInfoService(IBaseRepository<MainContext, ContactInfo> contactInfoRepository, IMessageGeneratorService messageGeneratorService)
        {
            this.contactInfoRepository = contactInfoRepository;
            this.messageGeneratorService = messageGeneratorService;
        }
        public PagedListResponse<ContactInfo> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<ContactInfo>();
            var list = contactInfoRepository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public ContactInfo Get(int id)
        {
            return contactInfoRepository.Get().FirstOrDefault(c => c.Id == id);
        }
        public GetResponse<ContactInfo> GetResponse(int id)
        {
            var data = contactInfoRepository.Get().FirstOrDefault(c => c.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Contact Info", MessageGeneratorActions.NotFound);
                return new GetResponse<ContactInfo>(message);
            }

            var response = new GetResponse<ContactInfo>();
            response.SetData(data);
            return response;
        }
        public EmptyResponse UpdateResponse(ContactInfo value)
        {
            contactInfoRepository.Table.Update(value);
            var dbResult = contactInfoRepository.Save();

            return new EmptyResponse(dbResult);
        }


        #region UnUsed
        public IQueryable<ContactInfo> GetList()
        {
            throw new System.NotImplementedException();
        }

        public ListResponse<ContactInfo> GetListResponse()
        {
            throw new System.NotImplementedException();
        }

        public CreateResponse CreateResponse(ContactInfo value)
        {
            throw new System.NotImplementedException();
        }

        public EmptyResponse DeleteResponse(int id)
        {
            throw new System.NotImplementedException();
        }

        public GetResponse<object> GetCountResponse()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
