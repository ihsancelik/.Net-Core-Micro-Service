using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Database.Models
{
    public class FeedBack
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public string  Rate { get; set; }

        [MiracleRequired]
        public string Options { get; set; }

        [MiracleRequired]
        public string SelectedProduct { get; set; }

        [MiracleRequired]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserName { get; set; }
    }
}