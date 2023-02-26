using Microsoft.AspNetCore.Authentication.Cookies;

namespace Library.Helpers.Attributes
{
    public class MiracleCookieAuthorizeAttribute : MiracleAuthorizeAttribute
    {
        public MiracleCookieAuthorizeAttribute(params string[] roles) : base(roles)
        {
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
