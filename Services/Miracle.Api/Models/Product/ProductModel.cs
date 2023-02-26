using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Product
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired, MaxLength(164)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(32)]
        public string Tag { get; set; }

        [MiracleRequired]
        public int VersionId { get; set; }

        [MiracleRequired]
        public double Price { get; set; }

        [MiracleRequired]
        public string Currency { get; set; }

        [MiracleRequired]
        public int Order { get; set; }

        [MiracleRequired, MaxLength(1024)]
        public string Description { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [MiracleRequired]
        public bool IsActive { get; set; }

        public IFormFile ProductImage { get; set; }
    }
}
