using Miracle.Api.Database.Models;
using System.Collections.Generic;

namespace Miracle.Api.Services
{
    public interface IFeedBackService : IBaseService<FeedBack>
    {
        public List<string> GetOptions();
    }
}