using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;

namespace Miracle.Api.Services
{
    public interface ISliderService : IBaseService<Slider>
    {
        public GetResponse<string> GetSliderImagePath(int id);
    }
}
