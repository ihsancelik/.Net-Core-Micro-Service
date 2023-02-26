using Library.Responses.Database;
using Library.Responses.Pagination;
using System;
using System.Collections.Generic;

namespace Library.Responses.Common
{
    public class PagedListResponse<T> : BaseResponse
        where T : class
    {
        public PagedResponse<T> PagedList { get; private set; }
        public PagedListResponse()
        {
        }
        public PagedListResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public PagedListResponse(bool success, string message = "") : base(success, message)
        {
        }
        public PagedListResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public PagedListResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public PagedListResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public PagedListResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(PagedResponse<T> pagedList)
        {
            PagedList = pagedList;
        }
    }
}
