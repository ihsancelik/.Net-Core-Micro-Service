using Library.Helpers.Database;
using Microsoft.Extensions.Configuration;

namespace Auth.Api.Helpers
{
    /// <summary>
    /// Debug is DevDb con str
    /// |
    /// Release is Db con str
    /// </summary>
    public class SQLConnectionStrings
    {
        private readonly IConfiguration configuration;
        public string ConnectionString
        {
            get
            {
                var value = configuration.GetSection("Db").Value;
#if DEBUG
                value = configuration.GetSection("DevDb").Value;
#endif
                return value;
            }
        }
        public string CoreConnectionString
        {
            get
            {
                var value = configuration.GetSection("CoreDb").Value;
#if DEBUG
                value = configuration.GetSection("DevCoreDb").Value;
#endif
                return value;
            }
        }
        public SQLConnectionStrings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
