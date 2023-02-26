using Auth.Api.Services;
using Library.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Api.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AuthService authService;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public object PageResponse { get; set; }
        public object DevInfo { get; set; }

        public IndexModel(AuthService authService)
        {
            this.authService = authService;
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Admin/Index");
            }
        }

        public async Task OnPostAsync()
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            var result = await authService.LoginWithCookieAsync(HttpContext, Username, SHA512Encryptor.Encrypt(Password), userAgent);

            if (!result)
            {
                PageResponse = authService.Exception;
                return;
            }


        }
    }
}
