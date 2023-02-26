using Library.Helpers.Attributes;

namespace Miracle.Api.Models.User
{
    public class UserProductVersionModel
    {
        [MiracleRequired]
        public int[] VersionInfoIdList { get; set; }
    }
}
