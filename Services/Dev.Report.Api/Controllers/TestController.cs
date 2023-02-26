using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Report.Api.Controllers
{
    [ApiController, Route("test")]
    public class TestController : ControllerBase
    {
        [Route("test")]
        public object Test()
        {
            return "ok";
        }

        [Route("test1"), MiracleAuthorize]
        public object Test1()
        {
            return "authed token!";
        }

        [Route("test2"), MiracleCookieAuthorize]
        public object Test2()
        {
            return "authed cookie!";
        }
    }
}
