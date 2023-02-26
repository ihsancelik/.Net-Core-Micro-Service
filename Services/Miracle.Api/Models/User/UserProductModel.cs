using Library.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.User
{
    public class UserProductModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        [MiracleRequired]
        public bool IsDemo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DemoStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DemoEndDate { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public IEnumerable<int> ProductIdList { get; set; }
    }
}
