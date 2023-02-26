
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
    public class PurchaseService : IPurchaseService
    {
        private readonly IBaseRepository<MainContext, Purchase> purchaseRepository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public PurchaseService(IBaseRepository<MainContext, Purchase> purchaseRepository, IMessageGeneratorService messageGeneratorService)
        {
            this.purchaseRepository = purchaseRepository;
            this.messageGeneratorService = messageGeneratorService;
        }

        public PagedListResponse<Purchase> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<Purchase>();
            var list = purchaseRepository.Table.GetPaged(model);
            response.SetData(list);
            return response;
        }
        public Purchase Get(int id)
        {
            return purchaseRepository.Get().FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<Purchase> GetList()
        {
            return purchaseRepository.Table;
        }
        public ListResponse<Purchase> GetListResponse()
        {
            var list = purchaseRepository.Table.ToList();
            var response = new ListResponse<Purchase>();
            response.SetData(list);
            return response;
        }
        public GetResponse<Purchase> GetResponse(int id)
        {
            var data = purchaseRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Purchase", MessageGeneratorActions.NotFound);
                return new GetResponse<Purchase>(message);
            }

            var response = new GetResponse<Purchase>();
            response.SetData(data);
            return response;
        }
        public CreateResponse CreateResponse(Purchase value)
        {
            purchaseRepository.Create(value);

            var dbResult = purchaseRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);
            return response;
        }
        public EmptyResponse UpdateResponse(Purchase value)
        {
            purchaseRepository.Table.Update(value);
            var dbResult = purchaseRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            var data = purchaseRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("Purchase", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            purchaseRepository.Table.Remove(data);
            var dbResult = purchaseRepository.Save();

            return new EmptyResponse(dbResult); ;
        }

        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(purchaseRepository.Table.Count());
            return response;
        }

    }
}
