using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Models.Currency;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System.Collections.Generic;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.Market), ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IMarketService marketService;
        private readonly CurrencyService currencyService;

        public MarketController(IMarketService marketService, CurrencyService currencyService)
        {
            this.marketService = marketService;
            this.currencyService = currencyService;
        }

        [HttpGet, MiracleAuthorize, Route(MarketRoutes.ReadyToPurchase)]
        public EmptyResponse ReadyToPurchase()
        {
            return new EmptyResponse();
        }

        [HttpPost, Route("getCurrency")]
        public ListResponse<CurrencyModel> GetCurrency([FromBody]List<string> codes)
        {
            if (ModelState.IsValid)
                return currencyService.GetCurrency(codes);

            return new ListResponse<CurrencyModel>(this.GetModelStateErrors());
        }
    }
}