using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miracle.Core.Api.Database.Models
{
    public class UserWatch
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastPingTime { get; set; }
        public bool Online { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public UserWatch(int userId)
        {
            UserId = userId;
            LastPingTime = DateTime.UtcNow;
        }
        public void StillOnline()
        {
            Online = true;
            LastPingTime = DateTime.UtcNow;
        }
        public void Offline()
        {
            Online = false;
        }

    }
}
