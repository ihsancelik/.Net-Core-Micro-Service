using Library.Helpers.Constraints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Library.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class MiracleAuthorizeAttribute : AuthorizeAttribute
    {
        public MiracleAuthorizeAttribute(params string[] roles)
        {
            if (roles.Length > 0)
                Roles = CombineRoles(roles);
            else
                Roles = CombineRoles(RoleConstraints.GetRoles());

            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public static string CombineRoles(IEnumerable<string> roles)
        {
            string role = string.Empty;

            foreach (var item in roles)
                role += item + ",";

            role = role.Substring(0, role.Length - 1);

            return role;
        }
    }
}
