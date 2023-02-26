using Library.Helpers.Attributes;
using Miracle.Api.Database.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Api.Models.Market
{
    public class CartModel
    {
        public IEnumerable<Purchase> Orders { get; set; }
    }

}
