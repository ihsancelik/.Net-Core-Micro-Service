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
    public class SmtpSettingService : ISmtpSettingService
    {
        private readonly IBaseRepository<MainContext, SmtpSetting> smtpSettingRepository;
        private readonly IMessageGeneratorService messageGeneratorService;

        public SmtpSettingService(IBaseRepository<MainContext, SmtpSetting> smtpSettingRepository, IMessageGeneratorService messageGeneratorService)
        {
            this.smtpSettingRepository = smtpSettingRepository;
            this.messageGeneratorService = messageGeneratorService;
        }
        #region Common
        public PagedListResponse<SmtpSetting> GetPagedListResponse(PaginationParameterModel model)
        {
            var response = new PagedListResponse<SmtpSetting>();
            var data = smtpSettingRepository.Table.GetPaged(model);
            response.SetData(data);
            return response;
        }
        public SmtpSetting Get(int id)
        {
            return smtpSettingRepository.Get().FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<SmtpSetting> GetList()
        {
            return smtpSettingRepository.Table;
        }
        public GetResponse<SmtpSetting> GetResponse(int id)
        {
            var data = smtpSettingRepository.Get().FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("SmtpSetting", MessageGeneratorActions.NotFound);
                return new GetResponse<SmtpSetting>(message);
            }

            var response = new GetResponse<SmtpSetting>();
            response.SetData(data);
            return response;
        }
        public ListResponse<SmtpSetting> GetListResponse()
        {
            var list = smtpSettingRepository.Table.ToList();
            var response = new ListResponse<SmtpSetting>();
            response.SetData(list);
            return response;
        }
        public CreateResponse CreateResponse(SmtpSetting value)
        {
            var isExist = smtpSettingRepository.Get().Any(s => s.Email == value.Email);
            if (isExist)
            {
                var message = messageGeneratorService.PrepareResponseMessage("SmtpSetting", MessageGeneratorActions.Exist);
                return new CreateResponse(message);
            }

            smtpSettingRepository.Table.Add(value);
            var dbResult = smtpSettingRepository.Save();

            var response = new CreateResponse(dbResult);
            response.SetData(value.Id);

            return response;
        }
        public EmptyResponse UpdateResponse(SmtpSetting value)
        {
            var isExist = smtpSettingRepository.Get().Any(s => s.Id != value.Id && s.Email == value.Email);
            if (isExist)
            {
                var message = messageGeneratorService.PrepareResponseMessage("SmtpSetting", MessageGeneratorActions.Exist);
                return new EmptyResponse(message);
            }

            smtpSettingRepository.Table.Update(value);
            var dbResult = smtpSettingRepository.Save();

            return new EmptyResponse(dbResult);
        }
        public EmptyResponse DeleteResponse(int id)
        {
            SmtpSetting deleteMail = smtpSettingRepository.Get().FirstOrDefault(s => s.Id == id);

            if (deleteMail == null)
            {
                var message = messageGeneratorService.PrepareResponseMessage("SmtpSetting", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            smtpSettingRepository.Table.Remove(deleteMail);
            var dbResult = smtpSettingRepository.Save();

            return new EmptyResponse(dbResult);
        }
        #endregion

        #region UnUsed
        public GetResponse<object> GetCountResponse()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}