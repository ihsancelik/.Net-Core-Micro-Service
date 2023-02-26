using System;

namespace Miracle.Api.Database
{
    /// <summary>
    /// Veritabanı değişikliklerini kaydettiğ
    /// </summary>
    public class DBResult
    {
        public bool Success { get; set; }
        /// <summary>
        /// Temel veritabanına yazılan durum girişlerinin sayısı
        /// </summary>
        public int StateEntriesCount { get; set; }
        public Exception Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="stateEntriesCount">Temel veritabanına yazılan durum girişlerinin sayısı</param>
        /// <param name="exception"></param>
        public DBResult(bool success, int stateEntriesCount, Exception exception = null)
        {
            Success = success;
            StateEntriesCount = stateEntriesCount;
            Exception = exception;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="success"></param>
        /// <param name="stateEntriesCount">Temel veritabanına yazılan durum girişlerinin sayısı</param>
        public DBResult(Exception exception, bool success = false, int stateEntriesCount = 0)
        {
            Success = success;
            StateEntriesCount = stateEntriesCount;
            Exception = exception;
        }
    }
}
