using Microsoft.Extensions.Configuration;

namespace Library.Helpers.Database
{
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

        public SQLConnectionStrings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
