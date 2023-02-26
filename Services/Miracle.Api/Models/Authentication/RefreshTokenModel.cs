using Library.Helpers.Attributes;

namespace Miracle.Api.Models.Authentication
{
    public class RefreshTokenModel
    {
        [MiracleRequired]
        public string WebRefreshToken { get; set; }
    }
}
