using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class ProductUsageInfo
    {
        public int Id { get; set; }
        public int Minute { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastUsageDate { get; set; }
    }
}
