using Adapter.MiracleWorld.Models;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Routes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using static Library.Routes.ApiCoreRoutes;

namespace Adapter.MiracleWorld.Controllers
{
    [MiracleAuthorize, Route(ControllerRoutes.QTLog), ApiController]
    public class LogController : ControllerBase
    {
        private string userMachinesPath;
        public LogController(IWebHostEnvironment env)
        {
            userMachinesPath = Path.Combine(env.WebRootPath, "UserMachines");
        }

        [HttpGet, Route(QTLogRoutes.GetUserMachineInfo)]
        public string[] GetUserMachineInfo(string username)
        {
            var fileDir = Path.Combine(userMachinesPath, username);
            var filePath = Path.Combine(fileDir, "Machines.txt");

            var datas = System.IO.File.ReadAllLines(filePath);

            return datas;
        }

        [HttpPost, Route(QTLogRoutes.CreateUserMachineInfo)]
        public bool CreateUserMachineInfo([FromBody] UserMachineModel model)
        {
            if (ModelState.IsValid)
            {
                var username = this.GetUsername();

                var fileDir = Path.Combine(userMachinesPath, username);
                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);

                string userMachine = $"CPU:{model.CPU}|OS:{model.OS}|OSVersion:{model.OSVersion}|UUID:{model.UUID}";

                var filePath = Path.Combine(fileDir, "Machines.txt");

                var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
                string[] machines = System.IO.File.ReadAllLines(filePath);
                fileStream.Dispose();
                if (!machines.Contains(userMachine))
                    System.IO.File.AppendAllText(filePath, userMachine + "\n");

                return true;
            }

            return false;
        }
    }
}
