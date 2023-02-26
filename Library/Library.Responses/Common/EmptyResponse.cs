using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Common
{
    public class EmptyResponse : BaseResponse
    {
        public EmptyResponse()
        {
        }
        public EmptyResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public EmptyResponse(bool success, string message = "") : base(success, message)
        {
        }
        public EmptyResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public EmptyResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public EmptyResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public EmptyResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
    }
}
