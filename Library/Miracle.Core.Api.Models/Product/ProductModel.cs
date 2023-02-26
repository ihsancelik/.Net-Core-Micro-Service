using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(164)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Tag { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [MiracleRequired]
        public bool IsPlugin { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public int[] PlatformIdList { get; set; }
    }
}
