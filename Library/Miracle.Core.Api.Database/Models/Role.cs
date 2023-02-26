using Library.Helpers.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    [JsonObject(IsReference = true)]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(64)]
        public string Value { get; set; }
       
        public ICollection<User_Role> User_Roles { get; }
        public Role()
        {
            User_Roles = new Collection<User_Role>();
        }
    }
}