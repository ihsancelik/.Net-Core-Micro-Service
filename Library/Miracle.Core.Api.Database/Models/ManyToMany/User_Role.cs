using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Core.Api.Database.Models
{
    public class User_Role
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public User User { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Role Role { get; set; }
    }
}
