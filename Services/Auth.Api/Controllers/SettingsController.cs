using Auth.Api.Services;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Auth.Api.Controllers
{
    [ApiController, Route("settings"), Authorize()]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService settingsService;

        public SettingsController(SettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        [HttpPost, Route("changeTokenType")]
        public GetResponseObject ChangeTokenType([FromBody] ChangeTokenTypeModel model)
        {
            var response = new GetResponseObject();

            if (ModelState.IsValid)
            {
                var result = settingsService.ChangeTokenTypeState(model.TokenType, model.MultiUsage);

                if (result)
                    response.Message = settingsService.Message;
                else
                    response.AddErrorList(settingsService.Exception);

                return response;
            }

            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }
    }

    public class ChangeTokenTypeModel
    {
        [Required]
        public string TokenType { get; set; }
        public bool MultiUsage { get; set; }
    }
}
