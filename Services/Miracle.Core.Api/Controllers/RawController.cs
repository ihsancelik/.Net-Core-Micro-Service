using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Miracle.Core.Api.Database;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Miracle.Core.Api.Controllers
{
    [Route("api/raw")]
    [ApiController]
    public class RawController : ControllerBase
    {
        private readonly MainContext db;
        private readonly IWebHostEnvironment env;
        private ILogger log = Log.ForContext<RawController>();
        public RawController(MainContext db, IConfiguration configuration, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [Route("bulkCompany")]
        [HttpGet]
        public int BulkCompany()
        {
            for (int i = 0; i < 1000; i++)
            {
                db.Companies.Add(new Database.Models.Company()
                {
                    Address = Guid.NewGuid().ToString(),
                    Location = Guid.NewGuid().ToString(),
                    PhoneNumber = "00000000000",
                    Name = Guid.NewGuid().ToString(),
                });
            }

            return db.SaveChanges();
        }

        [Route("a")]
        [HttpGet]
        public void a()
        {
            var a = db.Users.Include(s => s.Company).ToList();
        }

        [Route("b")]
        [HttpGet, MiracleAuthorize("SD")]
        public void b()
        {
            var a = db.Users.Include(s => s.Company).ToList();
        }

        [Route("c")]
        [HttpGet]
        public string[] c()
        {
            var logFolder = Path.Combine(env.WebRootPath, "ApiLogs");

            var date = DateTime.Now;
            var year = date.Year.ToString();
            var month = date.Month.ToString();
            var day = date.Day.ToString();

            if (month.Length < 2)
                month = "0" + month;

            if (day.Length < 2)
                day = "0" + day;

            var yyyymmdd = $"{year}{month}{day}";


            var filename = string.Format($"LOG-MIDDLEWARE-{yyyymmdd}.txt");
            var logFile = Path.Combine(logFolder, filename);


            var logFileCopy = Path.Combine(logFolder, Path.GetRandomFileName() + ".txt");

            System.IO.File.Copy(logFile, logFileCopy);
            var strr = System.IO.File.ReadAllLines(logFileCopy);
            System.IO.File.Delete(logFileCopy);


            return strr;
        }

        [HttpGet, Route("cp")]
        public void CreateProduct()
        {
            var user = db.Users
                .Where(s => s.Id == 1)
                .Include(s => s.User_Products)
                .Include("User_Products.Product_VersionInfos.VersionInfo")
                .Include("User_Products.ProductLimitation")
                .Include("User_Products.Product")
                .FirstOrDefault();

            var products = user.User_Products.Select(s => s.Product);
            foreach (var product in products)
            {
                var product_Versions = product.ProductSettings;
                foreach (var product_Version in product_Versions)
                {
                    var version = product_Version.VersionInfo.Version;
                }
            }

            Debugger.Break();
        }
    }
}
