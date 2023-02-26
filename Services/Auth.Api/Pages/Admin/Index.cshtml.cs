using Auth.Api.Database;
using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using static Library.Helpers.Constraints.RoleConstraints;

namespace Auth.Api.Pages.Admin
{
    [MiracleCookieAuthorize]
    public class IndexModel : PageModel
    {
        private readonly DataContext db;
        public IList<TokenType> TokenTypes { get; set; }
        public IndexModel(DataContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            TokenTypes = db.TokenTypes.ToList();
        }
    }
}
