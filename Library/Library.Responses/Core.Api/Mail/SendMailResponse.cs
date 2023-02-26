using Library.Responses.Common;
using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Core.Api.Mail
{
    public class SendMailResponse : BaseResponse
    {
        public SendMailResponse() : base()
        {
        }
        public SendMailResponse(bool success) : base(success)
        {
        }
        public SendMailResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public SendMailResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public SendMailResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public SendMailResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public SendMailResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
    }
}