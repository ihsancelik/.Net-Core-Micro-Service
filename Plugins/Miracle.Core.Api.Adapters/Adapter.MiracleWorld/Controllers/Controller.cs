using Adapter.MiracleWorld.Models;
using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Responses.Common;
using Library.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using Miracle.Core.Api.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Library.Routes.ApiCoreRoutes;

namespace Adapter.MiracleWorld.Controllers
{
    [Route(ControllerRoutes.QT), ApiController]
    public class Controller : ControllerBase
    {
        private readonly MainContext db;
        private readonly ImageManagerService imageManagerService;

        public Controller(MainContext db, ImageManagerService imageManagerService)
        {
            this.db = db;
            this.imageManagerService = imageManagerService;
        }

        [HttpGet, Route(QTRoutes.GetProducts), MiracleAuthorize]
        public ListResponse<Product> GetProducts()
        {
            int userId = this.GetId();

            var products = db.User_Products
                .Where(s => s.UserId == userId)
                .Include(s => s.Product)
                .Include(s => s.ProductLimitation)
                .Where(s => s.ProductLimitation.IsActive == true)
                .Where(s => s.Product.IsPlugin == false)
                .Select(s => s.Product)
                .Where(s => s.IsActive == true)
                .ToArray();

            var response = new ListResponse<Product>();
            response.SetData(products);
            return response;
        }

        [HttpGet, Route(QTRoutes.GetProductVersions), MiracleAuthorize]
        public ListResponse<string> GetProductVersions([FromRoute] string productTag)
        {
            int userId = this.GetId();

            var product = db.Products.FirstOrDefault(s => s.Tag == productTag && s.IsPlugin == false);
            if (product == null)
                return new ListResponse<string>("Product not found!");

            var versionInfos = db.User_Versions
                .Where(s => s.UserId == userId && s.ProductId == product.Id)
                .Include(s => s.VersionInfo)
                .Select(s => s.VersionInfo)
                .ToArray();

            if (versionInfos == null || versionInfos.Length == default)
                return new ListResponse<string>("Versions not found!");

            List<Version> versionList = new List<Version>();
            foreach (var version in versionInfos)
                versionList.Add(new Version(version.Version));

            var versionStrings = versionList.OrderByDescending(s => s.Major)
                .ThenByDescending(s => s.Minor)
                .ThenByDescending(s => s.Build)
                .ThenByDescending(s => s.Revision)
                .Select(s => s.ToString())
                .ToList();

            var response = new ListResponse<string>();
            response.SetData(versionStrings);
            return response;
        }

        [HttpGet, Route(QTRoutes.Download), AllowAnonymous]
        public FileResult DownloadProduct([FromRoute] string productTag, [FromRoute] string platform, [FromRoute] string version, [FromRoute] bool isPlugin)
        {
            int platformId = db.Platforms.Where(s => s.Name == platform).Select(s => s.Id).FirstOrDefault();
            int productId = db.Products.Where(s => s.Tag == productTag && s.IsPlugin == isPlugin).Select(s => s.Id).FirstOrDefault();
            int versionId = db.VersionInfos.Where(s => s.Version == version).Select(s => s.Id).FirstOrDefault();

            var setupInfo = db.SetupInfos
                .Where(s => s.PlatformId == platformId && s.ProductId == productId && s.VersionInfoId == versionId)
                .FirstOrDefault();

            // Bir önceki versiyonu yükle
            if (setupInfo == null)
            {
                
            }

            var directory = setupInfo.Path;
            var fileName = setupInfo.Name + "." + setupInfo.Extension;
            var filePath = Path.Combine(directory, fileName);

            if (!System.IO.File.Exists(filePath))
                return null;

            Stream stream = new FileStream(filePath, FileMode.Open);
            return File(stream, "application/octet-stream");
        }


        [HttpGet, Route(QTRoutes.GetUserInfo), MiracleAuthorize]
        public GetResponseObject GetProfileImage([FromRoute] string username)
        {
            GetResponseObject response = null;

            var user = db.Users.FirstOrDefault(s => s.Username == username);

            var imageName = user.ImageName;
            if (string.IsNullOrEmpty(imageName))
                return new GetResponseObject("No Profile Image");

            var imagePath = Path.Combine(imageManagerService.ProfileImagePath, imageName);
            if (!System.IO.File.Exists(imagePath))
                return new GetResponseObject("Profile Image Not Found");

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var imageData = Convert.ToBase64String(imageBytes);

            response = new GetResponseObject();
            response.SetData(new
            {
                ProfileImage = imageData,
                user.Name,
                user.Surname
            });
            return response;
        }

        [HttpGet, Route(QTRoutes.GetUsageData), MiracleAuthorize]
        public GetResponseObject GetUsageData([FromRoute] string username, [FromRoute] string productTag)
        {
            var user = db.Users.FirstOrDefault(s => s.Username == username);
            if (user == null)
                return new GetResponseObject("User not found");

            var product = db.Products.FirstOrDefault(s => s.Tag == productTag);
            if (product == null)
                return new GetResponseObject("Product not found");

            var user_product = db.User_Products
                .Include(s => s.ProductLimitation)
                .FirstOrDefault(s => s.UserId == user.Id && s.ProductId == product.Id);

            var user_ProductInfos = db.User_ProductInfos
                .Include(s => s.ProductUsageInfo)
                .Where(s => s.UserId == user.Id && s.ProductId == product.Id)
                .ToList();

            if (user_ProductInfos.Count() == 0)
                return new GetResponseObject("usage info not found");

            var daysRemaining = 0;
            var daysRemainingSpan = user_product.ProductLimitation.DemoEndDate - DateTime.Now;
            if (daysRemainingSpan != null)
                daysRemaining = (int)daysRemainingSpan.Value.TotalDays;

            if (DateTime.Now < user_product.ProductLimitation.DemoStartDate)
                daysRemaining = 0;

            if (DateTime.Now > user_product.ProductLimitation.DemoEndDate)
                daysRemaining = 0;


            var response = new GetResponseObject();
            response.SetData(new
            {
                TotalUsageMinute = user_ProductInfos.Sum(s => s.ProductUsageInfo.Minute),
                LastUsageDate = user_ProductInfos.LastOrDefault().ProductUsageInfo.LastUsageDate.ToString("dd/MM/yyyy"),
                IsDemo = user_product.ProductLimitation.IsDemo,
                IsActive = user_product.ProductLimitation.IsActive,
                DaysRemaining = daysRemaining
            });
            return response;
        }
        [HttpPost, Route(QTRoutes.SetUsageData), MiracleAuthorize]
        public EmptyResponse SetUsageData([FromRoute] string username, [FromRoute] string productTag, [FromBody] ProductUsageInfoModel model)
        {
            if (model == null)
                return new EmptyResponse("Model cannot be null");

            var user = db.Users
                .Include(s => s.User_ProductInfos)
                .FirstOrDefault(s => s.Username == username);
            if (user == null)
                return new EmptyResponse("User not found");

            var product = db.Products.FirstOrDefault(s => s.Tag == productTag);
            if (product == null)
                return new EmptyResponse("Product not found");


            user.User_ProductInfos.Add(new User_ProductInfo()
            {
                UserId = user.Id,
                ProductId = product.Id,
                ProductUsageInfo = new ProductUsageInfo()
                {
                    LastUsageDate = model.LastUsageDate,
                    Minute = model.Minute
                }
            });

            db.Users.Update(user);
            var dbResult = db.Save();

            return new EmptyResponse(dbResult);
        }

        [HttpGet, Route(QTRoutes.GetNotice), MiracleAuthorize]
        public GetResponse<string> GetNotice()
        {
            var notice = db.Notices.OrderByDescending(s => s.Id).FirstOrDefault();
            if (notice == null)
                return new GetResponse<string>("Notice not found!");

            var response = new GetResponse<string>();
            response.SetData(notice.Text);
            return response;
        }

        [HttpGet, Route(QTRoutes.GetLatestVersion)]
        public GetResponse<string> GetLatestVersion([FromRoute] string productTag, [FromRoute] string platform)
        {
            var _platform = db.Platforms.FirstOrDefault(s => s.Name == platform);
            if (_platform == null)
                return new GetResponse<string>("Platform not found!");

            var product = db.Products.FirstOrDefault(s => s.Tag == productTag);
            if (product == null)
                return new GetResponse<string>("Product not found!");

            if (!product.IsActive)
                return new GetResponse<string>("Product is not active!");

            int platformId = _platform.Id;
            int productId = product.Id;

            var versions = db.SetupInfos
                .Include(s => s.VersionInfo)
                .Where(s => s.ProductId == productId && s.PlatformId == platformId)
                .Select(s => s.VersionInfo)
                .ToList();

            if (versions == null || versions.Count == 0)
                return new GetResponse<string>("VersionInfo not found!");

            List<Version> versionList = new List<Version>();
            foreach (var version in versions)
                versionList.Add(new Version(version.Version));

            var latestVersion = versionList.Max().ToString();

            var response = new GetResponse<string>();
            response.SetData(latestVersion);
            return response;
        }
    }
}
