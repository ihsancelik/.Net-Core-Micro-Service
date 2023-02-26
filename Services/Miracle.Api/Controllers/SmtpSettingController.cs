using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Models.SmtpSetting;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.SmtpSetting), MiracleAuthorize(Roles = Roles.Admin), ApiController]
    public class SmtpSettingController : ControllerBase
    {
        private readonly IMessageGeneratorService messageGeneratorService;
        private readonly DataHelper dataHelper;
        private readonly ISmtpSettingService smtpService;
        public SmtpSettingController(IMessageGeneratorService messageGeneratorService, DataHelper dataHelper, ISmtpSettingService smtpService)
        {
            this.messageGeneratorService = messageGeneratorService;
            this.dataHelper = dataHelper;
            this.smtpService = smtpService;
        }

        #region Common

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.GetById)]
        public GetResponse<SmtpSetting> GetById(int id)
        {
            var data = smtpService.Get(id);

            var response = new GetResponse<SmtpSetting>();
            response.SetData(data);

            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.List)]
        public PagedListResponse<SmtpSetting> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return smtpService.GetPagedListResponse(model);
            }
            return new PagedListResponse<SmtpSetting>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Create)]
        public CreateResponse Create(SmtpSettingModel model)
        {
            if (ModelState.IsValid)
            {
                SmtpSetting smtpSetting = new SmtpSetting();

                var result = dataHelper.FieldBinder(model, smtpSetting);
                return result ? smtpService.CreateResponse(smtpSetting) : new CreateResponse(dataHelper.Errors);
            }

            return new CreateResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public EmptyResponse Update([FromRoute] int id, SmtpSettingModel model)
        {
            if (ModelState.IsValid)
            {
                var data = smtpService.Get(id);
                if (data == null)
                {
                    var message = messageGeneratorService.PrepareResponseMessage("SMTPSetting value", MessageGeneratorActions.Exist);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? smtpService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return smtpService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        #endregion
    }
}