using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController, Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet, Route("test")]
        public object Test()
        {
            return "ok";
        }
        [HttpGet, Route("test1"), Authorize()]
        public object Test1()
        {
            return "ok authed token";
        }
        [HttpGet, Route("test2"), MiracleCookieAuthorize]
        public object Test2()
        {
            return "ok authed cookie";
        }
    }
}
