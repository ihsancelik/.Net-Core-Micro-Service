using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Models.SMTPSetting;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.SmtpSetting), MiracleAuthorize(Roles = Roles.SD), ApiController]
    public class SMTPSettingController : ControllerBase
    {
        private readonly DataHelper dataHelper;
        private readonly ISMTPSettingService smtpService;
        public SMTPSettingController(ISMTPSettingService smtpService)
        {
            dataHelper = new DataHelper();
            this.smtpService = smtpService;
        }

        #region Common
        [HttpGet, Route(CRUDRoutes.GetById)]
        public GetResponse<SMTPSetting> GetById(int id)
        {
            var data = smtpService.Get(id);

            var response = new GetResponse<SMTPSetting>();
            response.SetData(data);

            return response;
        }

        [HttpPost, Route(CRUDRoutes.List)]
        public PagedListResponse<SMTPSetting> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return smtpService.GetPagedListResponse(model);
            }
            return new PagedListResponse<SMTPSetting>(this.GetModelStateErrors());
        }

        [HttpPost, Route(CRUDRoutes.Create)]
        public EmptyResponse Create(SMTPSettingModel model)
        {
            if (ModelState.IsValid)
            {
                SMTPSetting smtpSetting = new SMTPSetting();

                var result = dataHelper.FieldBinder(model, smtpSetting);
                return result ? smtpService.CreateResponse(smtpSetting) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, Route(CRUDRoutes.Update)]
        public EmptyResponse Update(SMTPSettingModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = smtpService.Get(id);
                if (data == null)
                {
                    var message = MessageGenerator.Generate("SMTPSetting value", MessageGeneratorActions.Exist);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? smtpService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return smtpService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return smtpService.GetCountResponse();
        }
        #endregion
    }
}