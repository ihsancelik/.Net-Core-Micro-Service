using Library.Helpers.Attributes;

namespace Miracle.Core.Api.Models.User
{
    public class UserProductVersionModel
    {
        [MiracleRequired]
        public int[] VersionInfoIdList { get; set; }
    }
}
