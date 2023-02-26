using Library.Helpers.Attributes;

namespace Miracle.Core.Api.Models.Logging
{
    public class LoggingModel
    {
        public string Username { get; set; }

        [MiracleRequired]
        public string Year { get; set; }

        [MiracleRequired]
        public string Month { get; set; }

        [MiracleRequired]
        public string Day { get; set; }
    }
}
