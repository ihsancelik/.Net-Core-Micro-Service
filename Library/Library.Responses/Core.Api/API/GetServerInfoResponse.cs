using System;

namespace Library.Responses.Core.Api.API
{
    public class GetServerInfoResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public ulong TotalRequestCount { get; set; }
    }
}
