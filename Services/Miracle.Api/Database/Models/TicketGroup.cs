using Library.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class TicketGroup
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Title { get; set; }
        public string SelectedProduct { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public bool IsClosed { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; }

        public TicketGroup()
        {
            TicketMessages = new Collection<TicketMessage>();
        }
    }
}