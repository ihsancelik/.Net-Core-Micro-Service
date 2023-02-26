using System;
using System.IO;

namespace Library.Helpers.Constraints
{
    public static class ApiCorePathConstraints
    {
        public static string Current { get { return Directory.GetCurrentDirectory(); } }
        public static string WWWRoot { get { return Path.Combine(Current, "wwwroot"); } }
        public static string StaticFiles { get { return Path.Combine(WWWRoot, "StaticFiles"); } }
        public static string LibFiles { get { return Path.Combine(WWWRoot, "LibFiles"); } }
        public static string Temp { get { return Path.Combine(WWWRoot, "Temp"); } }

        public static void Initialize()
        {
            try
            {
                if (!Directory.Exists(WWWRoot))
                    Directory.CreateDirectory(WWWRoot);

                if (!Directory.Exists(StaticFiles))
                    Directory.CreateDirectory(StaticFiles);

                if (!Directory.Exists(LibFiles))
                    Directory.CreateDirectory(LibFiles);

                if (!Directory.Exists(Temp))
                    Directory.CreateDirectory(Temp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
