using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.News
{
    public class NewsModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Tags { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Title { get; set; }

        [MiracleRequired]
        public string Text { get; set; }
        public IFormFile NewsImage { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }
    }
}
