using Miracle.Api.Database.Models;
using Miracle.Api.Models.Market;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class MarketService : IMarketService
    {
        private readonly IProductService productService;
        private readonly IPurchaseService purchaseService;
        private readonly CurrencyService currencyService;

        public MarketService(IProductService productService, IPurchaseService purchaseService, CurrencyService currencyService)
        {
            this.productService = productService;
            this.purchaseService = purchaseService;
            this.currencyService = currencyService;
        }
       

    }
}
