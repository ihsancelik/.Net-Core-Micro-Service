using Library.Responses.Common;
using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Core.Api.Logging
{
    public class GetApiLogsResponse : BaseResponse
    {
        public IEnumerable<string> ApiLogs { get; set; }
        public GetApiLogsResponse()
        {
        }
        public GetApiLogsResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public GetApiLogsResponse(bool success, string message = "") : base(success, message)
        {
        }
        public GetApiLogsResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public GetApiLogsResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public GetApiLogsResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetApiLogsResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(IEnumerable<string> apiLogs)
        {
            ApiLogs = apiLogs;
        }
    }
}
