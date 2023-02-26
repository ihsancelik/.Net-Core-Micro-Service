using Miracle.Api.Database;
using Miracle.Api.Database.Pagination;
using Miracle.Api.Responses.Base;
using System;
using System.Collections.Generic;

namespace Miracle.Api.Responses.Common
{
    public class PagedListResponse<T> : BaseResponse where T : class
    {
        public PagedResult<T> PagedList { get; private set; }
        public PagedListResponse()
        {
        }
        public PagedListResponse(DBResult dbResult) : base(dbResult)
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
        public void SetData(PagedResult<T> pagedList)
        {
            PagedList = pagedList;
        }
    }
}
