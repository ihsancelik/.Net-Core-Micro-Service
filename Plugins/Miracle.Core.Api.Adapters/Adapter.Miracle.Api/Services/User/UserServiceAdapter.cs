using Library.Helpers.Message;
using Library.Responses.Common;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using System.Linq;

namespace Adapter.Miracle.Api.Services
{
    public class UserServiceAdapter : IUserServiceAdapter
    {
        private readonly MainContext db;
        public UserServiceAdapter(MainContext db)
        {
            this.db = db;
        }

        public EmptyResponse AddProductOutSource(int userId, string tag, int versionId)
        {
            var product = db.Products.FirstOrDefault(s => s.Tag == tag);
            var userVersion = db.User_Versions.FirstOrDefault(s => s.ProductId == product.Id && s.UserId == userId && s.VersionInfoId == versionId);
            var userProduct = db.User_Products.FirstOrDefault(s => s.UserId == userId && s.ProductId == product.Id);
            if (userVersion != null)
                return new EmptyResponse(MessageGenerator.Generate("Product", MessageGeneratorActions.Exist));

            var productLimitation = new ProductLimitation();
            productLimitation.IsDemo = false;
            productLimitation.IsActive = true;

            db.ProductLimitations.Add(productLimitation);
            db.Save();

            if (userProduct == null)
                db.User_Products.Add(new User_Product()
                {
                    UserId = userId,
                    ProductId = product.Id,
                    ProductLimitationId = productLimitation.Id
                });

            db.User_Versions.Add(new User_Version()
            {
                UserId = userId,
                ProductId = product.Id,
                VersionInfoId = versionId
            });

            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }
    }
}
