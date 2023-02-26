using Dev.Report.Api.Database;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dev.Report.Api.Controllers
{
    [ApiController, Route("log")]
    public class LogController : ControllerBase
    {
        private readonly DataContext db;

        public LogController(DataContext db)
        {
            this.db = db;
        }

        [HttpGet, Route("send"), MiracleAuthorize("AccessSendLog")]
        public GetResponseObject Send([FromBody] LogModel model)
        {
            var response = new GetResponseObject();
            if (ModelState.IsValid)
            {
                try
                {
                    db.AppLogs.Add(new AppLog()
                    {
                        Name = model.Name,
                        Version = model.Version,
                        Exception = model.Exception,
                        Description = model.Description
                    });
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.AddErrorList(ex.Message);
                }

                return response;
            }

            response.AddRangeErrorList(this.GetModelStateErrors());
            return response;
        }
    }

    public class LogModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public string Exception { get; set; }
        public string Description { get; set; }
    }
}
