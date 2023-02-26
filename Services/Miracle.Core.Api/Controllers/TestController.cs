using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Miracle.Core.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly MainContext db;

        public TestController(MainContext db)
        {
            this.db = db;
        }

        [Route("helloworld")]
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World!";
        }

        [Route("test")]
        [HttpGet, MiracleAuthorize]
        public string Test()
        {
            return "test";
        }


        [Route("test2"), HttpGet]
        public MainModel Test2()
        {
            var users = db.Users
                .Include(s => s.Company)

                .Include(s => s.User_Products)
                .ThenInclude(s => s.Product)

                .Include(s => s.User_Products)
                .ThenInclude(s => s.ProductLimitation)

                .Include(s => s.User_ProductInfos)
                .ThenInclude(s => s.ProductUsageInfo)
                .Include(s => s.User_Roles)
                .Where(s => s.Company.Name != "Advanced Information Technologies")
                .ToList();

            MainModel mainModel = new MainModel()
            {
                UserCount = users.Count,
                TotalActiveUser = users.Count(s => s.IsActive)
            };

            List<UserInfoModel> userInfoModels = new List<UserInfoModel>();

            try
            {
                foreach (var user in users)
                {

                    var totalUsageMinute = user.User_ProductInfos?.Select(s => s.ProductUsageInfo)?.Sum(s => s.Minute);
                    var productUsageInfos = user.User_ProductInfos?.Select(s => s.ProductUsageInfo).ToList();
                    DateTime firstUsageDate = new DateTime();
                    DateTime lastUsageDate = new DateTime();

                    if (productUsageInfos.Count > 0)
                    {
                        firstUsageDate = productUsageInfos.Min(s => s.LastUsageDate);
                        lastUsageDate = productUsageInfos.Max(s => s.LastUsageDate);
                        mainModel.TotalUsageHours += (float)totalUsageMinute / 60f;
                    }


                    var userinfo = new UserInfoModel()
                    {
                        Company = $"{user.Company.Name}",
                        Fullname = $"{user.Name} {user.Surname}",
                        TotalUsageMinute = $"{totalUsageMinute}",
                        FirstUsageDate = $"{firstUsageDate.ToShortDateString()}",
                        LastUsageDate = $"{lastUsageDate.ToShortDateString()}",
                    };


                    //mainModel.TotalActiveUserProduct += user.User_Products.Count(s => s.ProductLimitation.IsActive);
                    //mainModel.TotalActiveDemo += user.User_Products.Count(s => s.ProductLimitation.IsDemo);
                    mainModel.ContinuesDemoUser += user.User_Products.Any(s => s.ProductLimitation.DemoEndDate > DateTime.Now) ? 1 : 0;
                    mainModel.GhostUserCount += user.User_Products.Any(s => s.ProductLimitation.DemoEndDate > DateTime.Now) ? 0 : 1;

                    foreach (var userProduct in user.User_Products)
                    {
                        DateTime firstProductUsageDate = new DateTime();
                        DateTime lastProductUsageDate = new DateTime();
                        var tum = 0;
                        var pui = userProduct.Product.User_ProductInfos.Where(s => s.UserId == user.Id).Select(s => s.ProductUsageInfo).ToList();

                        if (pui.Count > 0)
                        {
                            firstProductUsageDate = pui.Min(s => s.LastUsageDate);
                            lastProductUsageDate = pui.Max(s => s.LastUsageDate);
                            tum = pui.Sum(s => s.Minute);
                        }

                        

                        userinfo.ProductInfoModels.Add(new ProductInfoModel()
                        {
                            Product = userProduct.Product.Name,
                            IsDemo = userProduct.ProductLimitation.IsDemo,
                            IsActive = userProduct.ProductLimitation.IsActive,
                            FirstUsageDate = firstProductUsageDate.ToShortDateString(),
                            LastUsageDate = lastProductUsageDate.ToShortDateString(),
                            TotalUsageMinute = tum.ToString()
                        });
                    }

                    userInfoModels.Add(userinfo);
                }
            }
            catch (Exception ex)
            {

            }




            userInfoModels = userInfoModels.OrderByDescending(s => s.TotalUsageMinute).ToList();

            mainModel.UserInfoModels = userInfoModels;

            return mainModel;
        }
    }

    public class MainModel
    {
        public int UserCount { get; set; }
        public int GhostUserCount { get; set; }
        public int ContinuesDemoUser { get; set; }
        public float TotalUsageHours { get; set; }
        public int TotalActiveUser { get; set; }
        //public int TotalActiveUserProduct { get; set; }
        //public int TotalActiveDemo { get; set; }
        public ICollection<UserInfoModel> UserInfoModels { get; set; }

        public MainModel()
        {
            UserInfoModels = new Collection<UserInfoModel>();
        }
    }
    public class UserInfoModel
    {
        public string Company { get; set; }
        public string Fullname { get; set; }
        public string TotalUsageMinute { get; set; }
        public string FirstUsageDate { get; set; }
        public string LastUsageDate { get; set; }

        public ICollection<ProductInfoModel> ProductInfoModels { get; set; }

        public UserInfoModel()
        {
            ProductInfoModels = new Collection<ProductInfoModel>();
        }
    }

    public class ProductInfoModel
    {
        public string Product { get; set; }
        public string TotalUsageMinute { get; set; }
        public bool IsDemo { get; set; }
        public bool IsActive { get; set; }
        public string FirstUsageDate { get; set; }
        public string LastUsageDate { get; set; }
    }
}