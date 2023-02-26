using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.ProductLimitation
{
    public class ProductLimitationModel
    {
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
