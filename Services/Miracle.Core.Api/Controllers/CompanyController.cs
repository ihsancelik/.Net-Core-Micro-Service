using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Models.Company;
using Miracle.Core.Api.Models.Pagination;
using Miracle.Core.Api.Services;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Company), ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly MainContext mainContext;
        private readonly DataHelper dataHelper;

        public CompanyController(ICompanyService companyService, MainContext mainContext)
        {
            this.companyService = companyService;
            this.mainContext = mainContext;
            dataHelper = new DataHelper();
        }

        #region Common
        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.GetById)]
        public GetResponse<Company> GetById(int id)
        {
            return companyService.GetResponse(id);
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.ListAll)]
        public ListResponse<Company> GetListAll()
        {
            var companyList = companyService.GetList();
            var response = new ListResponse<Company>();
            response.SetData(companyList);
            return response;
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.List)]
        public PagedListResponse<Company> GetList(PaginationParameterModel model)
        {
            if (ModelState.IsValid)
            {
                return companyService.GetPagedListResponse(model);
            }
            return new PagedListResponse<Company>(this.GetModelStateErrors());
        }

        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Create)]
        public EmptyResponse Create(CompanyModel model)
        {
            if (ModelState.IsValid)
            {
                var company = new Company();

                var result = dataHelper.FieldBinder(model, company);
                return result ? companyService.CreateResponse(company) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpPut, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Update)]
        public EmptyResponse Update(CompanyModel model, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var data = companyService.Get(id);

                if (data == null)
                {
                    var message = MessageGenerator.Generate("Company value", MessageGeneratorActions.NotFound);
                    return new EmptyResponse(message);
                }

                var result = dataHelper.FieldBinder(model, data);
                return result ? companyService.UpdateResponse(data) : new EmptyResponse(dataHelper.Errors);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpDelete, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Delete)]
        public EmptyResponse Delete(int id)
        {
            if (ModelState.IsValid)
            {
                return companyService.DeleteResponse(id);
            }

            return new EmptyResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize(Roles = Roles.SD), Route(CRUDRoutes.Count)]
        public GetResponse<object> GetCount()
        {
            return companyService.GetCountResponse();
        }
        #endregion
    }
}