using Library.Helpers.Attributes;

namespace Miracle.Core.Api.Models.VersionInfo
{
    public class VersionInfoModel
    {
        public int Id { get; set; }
        [MiracleRequired]
        public string Version { get; set; }

    }
}
