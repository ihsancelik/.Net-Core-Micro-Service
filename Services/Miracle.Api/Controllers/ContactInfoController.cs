using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Enums;
using Miracle.Api.Extensions;
using Miracle.Api.Models.ContactInfo;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.ContactInfo), ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService contactInfoService;
        private readonly DataHelper dataHelper;
        private readonly IMessageGeneratorService messageGeneratorService;

        public ContactInfoController(IContactInfoService contactInfoService,
            DataHelper dataHelper,
            IMessageGeneratorService messageGeneratorService)
        {
            this.contactInfoService = contactInfoService;
            this.dataHelper = dataHelper;
            this.messageGeneratorService = messageGeneratorService;
        }

        #region Common

        [HttpGet, Route(CRUDRoutes.GetById)]
        public GetResponse<ContactInfo> GetById(int id)
        {
            return contactInfoService.GetResponse(id);
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Update)]
        public EmptyResponse Update([FromRoute] int id, ContactInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var data = contactInfoService.Get(id);
                if (data == null)
                {
                    var message = messageGeneratorService.PrepareResponseMessage("Contact Info", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                if (!result)
                    return new EmptyResponse(dataHelper.Errors);


                return contactInfoService.UpdateResponse(data);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        #endregion
    }
}