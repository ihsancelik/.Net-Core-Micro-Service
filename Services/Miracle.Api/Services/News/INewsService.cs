using Miracle.Api.Database.Models;
using Miracle.Api.Responses.Common;

namespace Miracle.Api.Services
{
    public interface INewsService : IBaseService<News>
    {
        public ListResponse<News> GetNewsByTag(string tags);
        public GetResponse<string> GetNewsImagePath(int id);
    }
}