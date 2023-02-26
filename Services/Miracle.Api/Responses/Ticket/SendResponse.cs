using Miracle.Api.Database;
using Miracle.Api.Responses.Base;
using System;
using System.Collections.Generic;

namespace Miracle.Api.Responses.Ticket
{
    public class SendResponse : BaseResponse
    {
        public int Id { get; private set; }
        public SendResponse()
        {
        }
        public SendResponse(DBResult dbResult) : base(dbResult)
        {
        }
        public SendResponse(bool success, string message = "") : base(success, message)
        {
        }
        public SendResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public SendResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public SendResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public SendResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {

        }
        public void SetData(int id)
        {
            Id = id;
        }
    }
}
