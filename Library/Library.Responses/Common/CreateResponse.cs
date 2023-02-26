using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Common
{
    public class CreateResponse : BaseResponse
    {
        public int Id { get; private set; }
        public CreateResponse()
        {
        }
        public CreateResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public CreateResponse(bool success, string message = "") : base(success, message)
        {
        }
        public CreateResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public CreateResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public CreateResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public CreateResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(int id)
        {
            Id = id;
        }
    }
}
