using Library.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class LiveChat
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public string RoomName { get; set; }

        [MiracleRequired]
        public string CustomerName { get; set; }

        [MiracleRequired]
        public string Company { get; set; }
        public string ConnectionId { get; set; }

        [MiracleRequired]
        public bool IsConnected { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<LiveChatContent> LiveChatContents { get; }

        public LiveChat()
        {
            LiveChatContents = new Collection<LiveChatContent>();
        }
    }
}