using System;

namespace Library.Responses.Database
{
    public class DatabaseResponse
    {
        public bool Success { get; set; }
        public int StateEntriesCount { get; set; }
        public Exception Exception { get; set; }

        public DatabaseResponse()
        {
            Success = false;
            StateEntriesCount = 0;
            Exception = null;
        }

        public DatabaseResponse(bool success, int stateEntriesCount, Exception exception = null)
        {
            Success = success;
            StateEntriesCount = stateEntriesCount;
            Exception = exception;
        }

        public DatabaseResponse(Exception exception, bool success = false, int stateEntriesCount = 0)
        {
            Success = success;
            StateEntriesCount = stateEntriesCount;
            Exception = exception;
        }
    }
}