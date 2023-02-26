using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Helpers.Message;
using Library.Responses.Core.Api.Logging;
using Library.Routes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.Models.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [Route(ControllerRoutes.Logging), MiracleAuthorize(Roles = Roles.SD), ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public LoggingController(IWebHostEnvironment env)
        {
            this.env = env;
        }


        [HttpPost, MiracleAuthorize(Roles = Roles.SD), Route(LoggingRoutes.GetApiLogs)]
        public GetApiLogsResponse GetApiLogs(LoggingModel model)
        {
            if (ModelState.IsValid)
            {
                var logFolder = Path.Combine(env.WebRootPath, "ApiLogs");

                var yyyymmdd = $"{model.Year}{model.Month}{model.Day}";

                var filename = string.Format($"LOG-MIDDLEWARE-{yyyymmdd}.txt");
                var logFile = Path.Combine(logFolder, filename);

                if (!System.IO.File.Exists(logFile))
                {
                    var message = MessageGenerator.Generate("ApiLogs", MessageGeneratorActions.NotFound);
                    return new GetApiLogsResponse();
                }

                var logFileCopy = Path.Combine(logFolder, Path.GetRandomFileName() + ".txt");
                System.IO.File.Copy(logFile, logFileCopy);

                var logData = System.IO.File.ReadAllLines(logFileCopy).ToList();
                System.IO.File.Delete(logFileCopy);

                var filteredData = FilterData(logData, model.Username);

                var response = new GetApiLogsResponse();
                response.SetData(filteredData);
                return response;
            }

            return new GetApiLogsResponse(this.GetModelStateErrors());
        }

        private List<string> FilterData(List<string> datas, string username)
        {
            if (string.IsNullOrEmpty(username))
                return datas;

            var newData = new List<string>();
            foreach (var data in datas)
            {
                var splitDatas = data.Split(',');
                if (splitDatas.Count() < 3)
                    continue;

                if (splitDatas[2] == username)
                    newData.Add(data);
            }
            return newData;
        }
    }
}