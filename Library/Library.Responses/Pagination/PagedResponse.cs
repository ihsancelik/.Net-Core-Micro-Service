using System.Collections.Generic;

namespace Library.Responses.Pagination
{
    public class PagedResponse<T> : BasePagedResponse where T : class
    {
        public IList<T> List { get; set; }

        public PagedResponse()
        {
            List = new List<T>();
        }
    }
}