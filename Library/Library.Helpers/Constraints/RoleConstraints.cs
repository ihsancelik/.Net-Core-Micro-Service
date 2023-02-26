using System.Collections.Generic;

namespace Library.Helpers.Constraints
{
    public static class RoleConstraints
    {
        public static class Roles
        {
            public const string SD = "SoftwareDeveloper";
            public const string Admin = "Admin";
            public const string ApiUser = "ApiUser";
            public const string User = "User";
            public const string AuthApiAdmin = "AuthApiAdmin";
        }

        public static List<string> GetRoles()
        {
            return new List<string>()
                {
                    new string(Roles.SD),
                    new string(Roles.Admin),
                    new string(Roles.ApiUser),
                    new string(Roles.User),
                    new string(Roles.AuthApiAdmin)
                };
        }
    }
}
