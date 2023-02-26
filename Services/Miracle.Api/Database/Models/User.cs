using Library.Helpers.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
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

        public string ImageName { get; set; }


        [MiracleRequired]
        public bool IsActive { get; set; }

        public string WebToken { get; set; }
        public string WebRefreshToken { get; set; }
        public DateTime? WebTokenExpire { get; set; }
        public DateTime? WebRefreshTokenExpire { get; set; }


        public User()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}