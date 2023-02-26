using Library.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }

        [MiracleRequired, MaxLength(128)]
        public string Name { get; set; }

        [MiracleRequired, MaxLength(512)]
        public string Address { get; set; }
        public string Location { get; set; }

        [MiracleRequired, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}