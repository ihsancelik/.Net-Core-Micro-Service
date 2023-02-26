using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Common
{
    public class GetResponseObject : BaseResponse
    {
        public object Data { get; private set; }
        public GetResponseObject()
        {
        }
        public GetResponseObject(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public GetResponseObject(bool success, string message = "") : base(success, message)
        {
        }
        public GetResponseObject(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public GetResponseObject(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public GetResponseObject(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetResponseObject(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(object data)
        {
            Data = data;
        }
    }
}
