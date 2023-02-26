using Library.Helpers.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miracle.Core.Api.Database.Models
{
    [JsonObject(IsReference = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Guid { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Surname { get; set; }

        [MiracleRequired]
        public string PhoneNumber { get; set; }

        [MiracleRequired, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Username { get; set; }

        [MiracleRequired, DataType(DataType.Password)]
        public string Password { get; set; }

        public string ImageName { get; set; }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string WebToken { get; set; }
        public string WebRefreshToken { get; set; }

        public DateTime? TokenExpire { get; set; }
        public DateTime? WebTokenExpire { get; set; }
        public DateTime? RefreshTokenExpire { get; set; }
        public DateTime? WebRefreshTokenExpire { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [ForeignKey("Priority")]
        public int? PriorityId { get; set; }
        public Priority Priority { get; set; }

        public ICollection<User_Role> User_Roles { get; }
        public ICollection<User_Product> User_Products { get; }
        public ICollection<User_Version> User_Versions { get; }
        public ICollection<User_ProductInfo> User_ProductInfos { get; set; }
        public ICollection<ResetPassword> ResetPasswords { get; }
        public ICollection<User_Product_Module> User_Product_Modules { get; }

        public User()
        {
            User_Roles = new Collection<User_Role>();
            User_Products = new Collection<User_Product>();
            User_ProductInfos = new Collection<User_ProductInfo>();
            User_Versions = new Collection<User_Version>();
            ResetPasswords = new Collection<ResetPassword>();
            User_Product_Modules = new Collection<User_Product_Module>();

            Guid = System.Guid.NewGuid().ToString();
        }
    }
}