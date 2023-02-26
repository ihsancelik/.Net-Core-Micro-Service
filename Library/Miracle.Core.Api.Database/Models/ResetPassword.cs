using Library.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miracle.Core.Api.Database.Models
{
    public class ResetPassword
    {
        [Key]
        public int Id { get; set; }

        [MiracleRequired]
        public string Code { get; set; }

        [MiracleRequired, DataType(DataType.DateTime)]
        public DateTime ExpireDate { get; set; }

        [MiracleRequired]
        public bool IsUsed { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}