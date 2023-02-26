using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Package.Manager.Api.Constraints
{
    public abstract class PathConstraints
    {
        public static string Current { get { return Directory.GetCurrentDirectory(); } }
        public static string WWWRoot { get { return Path.Combine(Current, "wwwroot"); } }
        public static string StaticFiles { get { return Path.Combine(WWWRoot, "StaticFiles"); } }
        public static string MPM { get { return Path.Combine(StaticFiles, "mpm"); } }
        public static string TEMP { get { return Path.Combine(StaticFiles, "temp"); } }
        public static string MPM_Libs { get { return Path.Combine(MPM, "libs"); } }
        public static string MPM_Products { get { return Path.Combine(MPM, "products"); } }
    }
    public abstract class StaticFileOptionsConstraints
    {
        public const string RequestPath = "/libs";
        public static PhysicalFileProvider FileProvider { get { return new PhysicalFileProvider(PathConstraints.MPM_Libs); } }
    }
    public abstract class RequestSizeConstraints
    {
        public const long MultipartBodyLengthLimit = 5000000000;
        public const long RequestSizeLimit = 5000000000;
    }
}
