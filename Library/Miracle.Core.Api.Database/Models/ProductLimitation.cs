using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class ProductLimitation
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public bool IsDemo { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DemoStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DemoEndDate { get; set; }
        [MiracleRequired]
        public bool IsActive { get; set; }
    }
}
