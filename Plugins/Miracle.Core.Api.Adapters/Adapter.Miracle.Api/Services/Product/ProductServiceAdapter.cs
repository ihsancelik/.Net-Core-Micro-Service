using Library.Responses.Common;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using System.Linq;

namespace Adapter.Miracle.Api.Services
{
    public class ProductServiceAdapter : IProductServiceAdapter
    {
        private readonly MainContext db;

        public ProductServiceAdapter(MainContext db)
        {
            this.db = db;
        }

        public ListResponse<object> GetProducts()
        {
            var productsModel = db.Products.Include(s=>s.ProductSettings).Select(s => new { s.Id, s.Name, s.Tag, s.PublishDate,s.ProductSettings }).ToList();
            var response = new ListResponse<object>();
            response.SetData(productsModel);
            return response;
        }
    }
}
