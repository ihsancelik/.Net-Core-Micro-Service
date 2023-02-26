using Library.Helpers.Attributes;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Extensions;
using Miracle.Api.Models.ContactForm;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Api.Controllers
{

    [Route(ControllerRoutes.ContactForm), ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService contactFormService;
        private readonly DataHelper dataHelper;

        public ContactFormController(IContactFormService contactFormService, DataHelper dataHelper)
        {
            this.contactFormService = contactFormService;
            this.dataHelper = dataHelper;
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.GetById)]
        public GetResponse<ContactForm> GetById(int id)
        {
            return contactFormService.GetResponse(id);
        }

        [HttpGet, Route(CRUDRoutes.ListAll)]
        public ListResponse<ContactForm> GetListAll()
        {
            var contactFormList = contactFormService.GetList();
            var response = new ListResponse<ContactForm>();
            response.SetData(contactFormList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.List)]
        public PagedListResponse<ContactForm> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return contactFormService.GetPagedListResponse(model);
            }

            return new PagedListResponse<ContactForm>(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var response = contactFormService.DeleteResponse(id);
                return response;
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.Admin), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return contactFormService.GetCountResponse();
        }

        #endregion

        [HttpPost, Route(CRUDRoutes.Create)]
        public CreateResponse Send(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                ContactForm contactForm = new ContactForm();
                var result = dataHelper.FieldBinder(model, contactForm);

                if (!result)
                    return new CreateResponse(dataHelper.Errors);

                var response = contactFormService.CreateResponse(contactForm);
                return response;
            }

            return new CreateResponse(this.GetModelStateErrors());
        }
    }
}