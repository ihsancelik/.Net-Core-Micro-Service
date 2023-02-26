using Library.Helpers.Attributes;
using Library.Helpers.Message;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Core.Api.StaticDatas;
using System.Linq;
using static Library.Helpers.Constraints.RoleConstraints;
using static Library.Routes.ApiCoreRoutes;

namespace Miracle.Core.Api.Controllers
{
    [ApiController, Route(ControllerRoutes.ServerInfo), MiracleAuthorize(Roles.SD)]
    public class ServerInfoController : ControllerBase
    {
        [HttpGet, Route(ServerInfoRoutes.GetInfo)]
        public GetResponseObject GetInfo()
        {
            var response = new GetResponseObject();
            response.SetData(new
            {
                StartDate = StaticDataServerInfo.StartDate,
                CurrentDate = StaticDataServerInfo.CurrentDate,
                TotalRequestCount = StaticDataServerInfo.TotalRequestCount
            });
            return response;
        }

        [HttpGet, Route(ServerInfoRoutes.GetDependencyExceptions)]
        public ListResponse<string> GetDependencyExceptions(string libName)
        {
            var exceptions = StaticDataServerInfo.DependencyExceptions
              .Where(s => s.LibName == libName)
              .FirstOrDefault()?
              .Exceptions;

            if (exceptions == null)
                return new ListResponse<string>(MessageGenerator.Generate(libName, MessageGeneratorActions.NotFound));

            var exceptionMessages = exceptions.Select(s => s.Message);

            var response = new ListResponse<string>();
            response.SetData(exceptionMessages);

            return response;
        }
    }
}