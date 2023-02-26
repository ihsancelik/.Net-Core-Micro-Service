using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Api.Database.Models
{
    public class LiveChatContent
    {
        [Key]
        public int Id { get; set; }

        public string AdminName { get; set; }

        [MiracleRequired]
        public string Message { get; set; }

        [MiracleRequired]
        public bool IsAdmin { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("LiveChat")]
        public int LiveChatId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public LiveChat LiveChat { get; set; }
    }
}