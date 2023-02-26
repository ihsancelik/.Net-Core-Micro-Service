using System;
using System.Collections.Generic;

namespace Miracle.Core.Api.StaticDatas
{
    public abstract class StaticDataServerInfo
    {
        public static DateTime StartDate { get; private set; }
        public static DateTime CurrentDate { get { return DateTime.Now; } }
        public static ulong TotalRequestCount { get; private set; }
        public static List<DependencyManagerException> DependencyExceptions { get; set; }

        public static void Initialize()
        {
            StartDate = DateTime.Now;
            TotalRequestCount = 0;
            DependencyExceptions = new List<DependencyManagerException>();
        }

        public static void SetStartDate(DateTime date)
        {
            StartDate = date;
        }
        public static void AddTotalRequestCount()
        {
            TotalRequestCount += 1;
        }
    }
}
