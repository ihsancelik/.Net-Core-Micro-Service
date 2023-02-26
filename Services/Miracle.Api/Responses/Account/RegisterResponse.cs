using Miracle.Api.Database;
using Miracle.Api.Responses.Base;
using System;
using System.Collections.Generic;

namespace Miracle.Api.Responses.Account
{
    public class RegisterResponse : BaseResponse
    {
        public RegisterResponse()
        {
        }

        public RegisterResponse(DBResult dbResult) : base(dbResult)
        {
        }

        public RegisterResponse(bool success, string message = "") : base(success, message)
        {
        }

        public RegisterResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }

        public RegisterResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }

        public RegisterResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }

        public RegisterResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
    }
}
