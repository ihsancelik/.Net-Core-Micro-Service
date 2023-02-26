using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Notice
{
    public class NoticeModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Text { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
    }
}