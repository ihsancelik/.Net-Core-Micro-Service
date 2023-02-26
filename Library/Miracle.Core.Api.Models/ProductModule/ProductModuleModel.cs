using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.ProductModule
{
    public class ProductModuleModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(164)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Description { get; set; }
    }
}
