using Library.Responses.Common;
using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Core.Api.User
{
    public class GetUserResponse<T> : BaseResponse where T : class
    {
        public T Data { get; set; }
        public GetUserResponse() : base()
        {
        }
        public GetUserResponse(bool success) : base(success)
        {
        }
        public GetUserResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public GetUserResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public GetUserResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetUserResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetUserResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public void SetData(T data)
        {
            Data = data;
        }
    }
}
