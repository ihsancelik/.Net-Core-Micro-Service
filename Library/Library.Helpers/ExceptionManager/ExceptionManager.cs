using System;
using System.Collections.Generic;

namespace Library.Helpers.ExceptionManager
{
    public class ExceptionManager
    {
        public bool HaveException { get; set; }
        public ICollection<Exception> Exceptions { get; private set; }

        public ExceptionManager()
        {
            Exceptions = new List<Exception>();
        }

        public void AddException(string exception)
        {
            Exceptions.Add(new Exception(exception));
            HaveException = true;
        }
        public void AddException(Exception exception)
        {
            Exceptions.Add(exception);
            HaveException = true;
        }
    }
}
