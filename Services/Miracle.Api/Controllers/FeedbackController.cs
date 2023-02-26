using Library.Helpers.Attributes;
using Library.Helpers.Extensions;
using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using Miracle.Api.Database.Models;
using Miracle.Api.Models;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services;
using Miracle.Api.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Library.Routes.ApiRoutes;

namespace Miracle.Api.Controllers
{
    [Route(ControllerRoutes.FeedBack), ApiController]

    public class FeedbackController : ControllerBase
    {
        private readonly IFeedBackService feedbackService;
        private readonly DataHelper dataHelper;

        public FeedbackController(IFeedBackService feedbackService, DataHelper dataHelper)
        {
            this.feedbackService = feedbackService;
            this.dataHelper = dataHelper;
        }

        [HttpGet, MiracleAuthorize, Route(FeedBackRoutes.GetOptions)]
        public List<string> GetOptions()
        {
            return feedbackService.GetOptions();
        }

        [HttpPost, MiracleAuthorize, Route(CRUDRoutes.Create)]
        public async Task<CreateResponse> Send([FromForm] FeedBackModel model)
        {
            if (ModelState.IsValid)
            {
                FeedBack feedBack = new FeedBack();
                feedBack.Options = model.Options;
                feedBack.Message = model.Message;
                feedBack.SelectedProduct = model.SelectedProduct;
                feedBack.Rate = model.Rate;
                feedBack.UserName = this.GetUsername();

                feedBack.Rate =model.Rate;

                var result = dataHelper.FieldBinder(model, feedBack);

                if (!result)
                    return new CreateResponse(dataHelper.Errors);

                var response = feedbackService.CreateResponse(feedBack);
                return response;
            }

            return new CreateResponse(this.GetModelStateErrors());
        }

        [HttpGet, MiracleAuthorize, Route(CRUDRoutes.ListAll)]
        public ListResponse<FeedBack> GetListAll()
        {
            var list = feedbackService.GetList();
            var response = new ListResponse<FeedBack>();
            response.SetData(list);

            return response;
        }
    }
}