using Miracle.Api.Database;
using Miracle.Api.Responses.Base;
using System;
using System.Collections.Generic;

namespace Miracle.Api.Responses.Common
{
    public class GetResponse<T> : BaseResponse where T : class
    {
        public T Data { get; set; }
        public GetResponse()
        {
        }
        public GetResponse(DBResult dbResult) : base(dbResult)
        {
        }
        public GetResponse(bool success, string message = "") : base(success, message)
        {
        }
        public GetResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public GetResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public GetResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(T data)
        {
            Data = data;
        }
    }
}
