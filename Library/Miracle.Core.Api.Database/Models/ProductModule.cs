using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Database.Models
{
    public class ProductModule
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product_Module> Product_Modules{ get; set; }

        public ProductModule()
        {
            Product_Modules = new Collection<Product_Module>();
        }
    }
}