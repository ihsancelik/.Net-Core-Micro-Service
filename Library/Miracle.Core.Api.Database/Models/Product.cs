using Library.Helpers.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    [JsonObject(IsReference = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(164)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Tag { get; set; }

        public bool IsPlugin { get; set; }


        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public ICollection<Platform_Product> Platform_Products { get; }
        public ICollection<User_Product> User_Products { get; }
        public ICollection<User_ProductInfo> User_ProductInfos { get; }
        public ICollection<User_Version> User_Versions { get; }
        public ICollection<ProductSetting> ProductSettings { get; }
        public ICollection<Product_Module> Product_Modules { get; }
        public ICollection<User_Product_Module> User_Product_Modules { get; set; }

        public Product()
        {
            Platform_Products = new Collection<Platform_Product>();
            User_Products = new Collection<User_Product>();
            User_ProductInfos = new Collection<User_ProductInfo>();
            User_Versions = new Collection<User_Version>();
            ProductSettings = new Collection<ProductSetting>();
            Product_Modules = new Collection<Product_Module>();
            User_Product_Modules = new Collection<User_Product_Module>();
        }
    }
}
