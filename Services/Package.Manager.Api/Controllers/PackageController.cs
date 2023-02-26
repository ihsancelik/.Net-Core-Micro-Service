using Library.Helpers.Attributes;
using Library.Helpers.Constraints;
using Library.Helpers.Extensions;
using Library.Helpers.Mapper;
using Library.Responses.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Package.Manager.Api.Constraints;
using Package.Manager.Api.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Package.Manager.Api.Controllers
{
    [ApiController, Route("package")]
    public class PackageController : ControllerBase
    {
        private readonly PackageService packageService;
        private readonly DataHelper dataHelper;

        public PackageController(PackageService packageService, DataHelper dataHelper)
        {
            this.packageService = packageService;
            this.dataHelper = dataHelper;
        }

        [HttpGet, Route("getAll"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        public object GetAll()
        {
            var packages = packageService.GetAll();
            var response = new GetResponse<List<Database.Package>>();
            response.SetData(packages);
            return response;
        }

        [HttpPost, Route("upload"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        [RequestFormLimits(MultipartBodyLengthLimit = RequestSizeConstraints.MultipartBodyLengthLimit)]
        [RequestSizeLimit(RequestSizeConstraints.RequestSizeLimit)]
        public GetResponse<string> Upload([FromForm] UploadModel model)
        {
            var response = new GetResponse<string>();
            if (ModelState.IsValid)
            {
                var packageServiceModel = new PackageServiceModel();
                var binderResult = dataHelper.FieldBinder(model, packageServiceModel, new List<string>() { "File" });
                if (!binderResult)
                {
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                var result = packageService.Create(packageServiceModel, model.File);
                if (!result)
                {
                    response.AddErrorList(packageService.Exception);
                    return response;
                }

                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route("isExist"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        public bool Exist(ExistModel model)
        {
            var packageServiceModel = new PackageServiceModel();
            dataHelper.FieldBinder(model, packageServiceModel);
            return packageService.Exist(packageServiceModel);
        }

        [HttpPost, Route("edit"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        public GetResponse<string> Edit(EditModel model)
        {
            var response = new GetResponse<string>();
            if (ModelState.IsValid)
            {
                var packageServiceModel = new PackageServiceModel();
                var binderResult = dataHelper.FieldBinder(model, packageServiceModel, new List<string>() { "File" });
                if (!binderResult)
                {
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                var result = packageService.Edit(packageServiceModel, model.File);
                if (!result)
                {
                    response.AddErrorList(packageService.Exception);
                    return response;
                }

                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route("delete"), MiracleAuthorize(RoleConstraints.Roles.SD)]
        public GetResponse<string> Delete(DeleteModel model)
        {
            var response = new GetResponse<string>();
            if (ModelState.IsValid)
            {
                var packageServiceModel = new PackageServiceModel();
                var binderResult = dataHelper.FieldBinder(model, packageServiceModel);
                if (!binderResult)
                {
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                var result = packageService.Delete(packageServiceModel);
                if (!result)
                {
                    response.AddErrorList(packageService.Exception);
                    return response;
                }

                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }

        [HttpPost, Route("getUrl"), AllowAnonymous]
        public GetResponse<string> GetUrl(GetUrlModel model)
        {
            var response = new GetResponse<string>();
            if (ModelState.IsValid)
            {
                var packageServiceModel = new PackageServiceModel();
                var binderResult = dataHelper.FieldBinder(model, packageServiceModel);
                if (!binderResult)
                {
                    response.AddRangeErrorList(dataHelper.Errors);
                    return response;
                }

                var fileUrl = packageService.GetPackageFileUrl(packageServiceModel);
                if (fileUrl == null)
                {
                    response.AddErrorList(packageService.Exception);
                    return response;
                }

                response.SetData(fileUrl);
                return response;
            }
            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }
    }

    public class EditModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
    public class UploadModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Version { get; set; }
    }
    public class ExistModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
    }
    public class GetUrlModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
    }
    public class DeleteModel
    {
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
    }
}
