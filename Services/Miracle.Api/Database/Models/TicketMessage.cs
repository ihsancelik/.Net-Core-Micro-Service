using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Miracle.Api.Database.Models
{
    public class TicketMessage
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Message { get; set; }
        public string ImageName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public bool IsAdmin { get; set; }
        public string AdminName { get; set; }

        public string UserName { get; set; }

        [ForeignKey("TicketGroup")]
        public int TicketGroupId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public TicketGroup TicketGroup { get; set; }
    }
}