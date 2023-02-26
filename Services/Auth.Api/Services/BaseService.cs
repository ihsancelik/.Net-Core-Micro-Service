using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Api.Services
{
    public class BaseService
    {
        public string Message;
        public string Exception = null;
        public int DatabaseNumberOfChanges = 0;
    }
}
