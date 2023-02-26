using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Miracle.Api.Services.Helpers
{
    /// <summary>
    /// Task adına cancellation token yaratmak için kullanılır.
    /// </summary>
    public class CancellationTokenManager
    {
        private List<CancellationTokenInfo> cancellationTokenInfos { get; }

        /// <summary>
        /// Yeni bir cancellation token yaratır.
        /// Cancellation token'ı id, name ve description
        /// parametreleri ile özelleştirebilirsiniz.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CancellationToken Create(int id, string name, string description)
        {
            var cancellationToken = new CancellationTokenInfo(id, name, description);
            cancellationTokenInfos.Add(cancellationToken);
            var token = cancellationToken.Token;
            return token;
        }

        public void CancelTask(int id)
        {
            var token = cancellationTokenInfos.FirstOrDefault(s => s.Id == id);
            if (token == null)
                return;
            token.Cancel();
            cancellationTokenInfos.Remove(token);
        }
        public void CancelTask(string name)
        {
            var token = cancellationTokenInfos.FirstOrDefault(s => s.Name == name);
            if (token == null)
                return;
            token.Cancel();
            cancellationTokenInfos.Remove(token);
        }
        public void CancelTask(CancellationToken value)
        {
            var token = cancellationTokenInfos.FirstOrDefault(s => s.Token == value);
            if (token == null)
                return;
            token.Cancel();
            cancellationTokenInfos.Remove(token);
        }
        public void CancelAllTasks()
        {
            var count = cancellationTokenInfos.Count;
            for (int i = 0; i < count; i++)
            {
                cancellationTokenInfos[i].Cancel();
            }
        }
    }
}
