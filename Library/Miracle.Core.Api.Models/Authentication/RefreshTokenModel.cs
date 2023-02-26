using Library.Helpers.Attributes;

namespace Miracle.Core.Api.Models.Authentication
{
    public class RefreshTokenModel
    {
        [MiracleRequired]
        public string RefreshToken { get; set; }
    }
}
