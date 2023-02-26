using System.Collections.Generic;

namespace Miracle.Api.Database.Pagination
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> List { get; set; }

        public PagedResult()
        {
            List = new List<T>();
        }
    }
}
