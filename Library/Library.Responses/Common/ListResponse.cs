﻿using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Common
{
    public class ListResponse<T> : BaseResponse where T : class
    {
        public IEnumerable<T> List { get; private set; }
        public ListResponse()
        {
        }
        public ListResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public ListResponse(bool success, string message = "") : base(success, message)
        {
        }
        public ListResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public ListResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public ListResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public ListResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(IEnumerable<T> list)
        {
            List = list;
        }
    }
}
