using Library.Routes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Miracle.Api.Controllers
{
    [Route("api/hello"), ApiController]
    public class HelloController : ControllerBase
    {
        [Route("hello")]
        public string Hello()
        {
            return "Hello";
        }
    }
}