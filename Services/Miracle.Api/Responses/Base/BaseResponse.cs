using Miracle.Api.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Api.Responses.Base
{
    /// <summary>
    /// Her response bu responsu referans almalıdır.
    /// </summary>
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<string> ErrorList { get; }

        public BaseResponse()
        {
            ErrorList = new List<string>();
            Success = true;
            Message = "Success";
        }
        public BaseResponse(bool success, string message = "")
        {
            ErrorList = new List<string>();
            Success = success;
            Message = message;
        }
        public BaseResponse(Exception exception, bool success = false)
        {
            ErrorList = new List<string>();
            Success = success;
            Message = $"Failed - {exception.Message}";
            AddErrorList(exception);
        }
        public BaseResponse(string exceptionMessage, bool success = false)
        {
            ErrorList = new List<string>();
            Success = success;
            Message = $"Failed - {exceptionMessage}";
            AddErrorList(exceptionMessage);
        }
        public BaseResponse(IEnumerable<Exception> exceptionMessages, bool success = false)
        {
            ErrorList = new List<string>();
            Success = success;
            Message = $"Failed - {exceptionMessages.FirstOrDefault()?.Message}";
            AddRangeErrorList(exceptionMessages);
        }
        public BaseResponse(IEnumerable<string> exceptionMessages, bool success = false)
        {
            ErrorList = new List<string>();
            Success = success;
            Message = $"Failed - {exceptionMessages.FirstOrDefault()}";
            AddRangeErrorList(exceptionMessages);
        }
        public BaseResponse(DBResult dbResult)
        {
            ErrorList = new List<string>();
            Success = dbResult.Success;
            Message = "Failed";

            if (Success)
                Message = "Success";

            if (dbResult.Exception != null)
            {
                var exception = dbResult.Exception;
                Message = $"Failed - {exception.Message}";
                AddErrorList(exception);
            }
        }


        public IEnumerable<string> GetErrorList()
        {
            return ErrorList;
        }
        public void AddErrorList(Exception exception)
        {
            ErrorList.Add(exception.Message);
        }
        public void AddRangeErrorList(IEnumerable<Exception> exceptions)
        {
            ErrorList.AddRange(exceptions.Select(s => s.Message));
        }
        public void AddErrorList(string exceptionMessage)
        {
            ErrorList.Add(exceptionMessage);
        }
        public void AddRangeErrorList(IEnumerable<string> exceptionMessages)
        {
            ErrorList.AddRange(exceptionMessages);
        }
        public void ClearErrorList()
        {
            ErrorList.Clear();
        }
    }
}
