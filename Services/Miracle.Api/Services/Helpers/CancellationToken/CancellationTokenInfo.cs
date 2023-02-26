using System.Threading;

namespace Miracle.Api.Services.Helpers
{
    /// <summary>
    /// CancellationTokenManager için model
    /// </summary>
    public class CancellationTokenInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private CancellationTokenSource source { get; set; }
        public CancellationToken Token { get; set; }
        public CancellationTokenInfo(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;

            source = new CancellationTokenSource();
            Token = source.Token;
        }
        public void Cancel()
        {
            source.Cancel();
        }
        public void Dispose()
        {
            source.Dispose();
        }
    }
}
