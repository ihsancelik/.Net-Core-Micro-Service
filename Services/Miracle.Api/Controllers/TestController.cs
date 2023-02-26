using Library.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Miracle.Api.Services.Helpers;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Miracle.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HTTPManagerService hTTPManagerService;
        private string BaseUrl;
        public TestController(IConfiguration configuration, HTTPManagerService hTTPManagerService)
        {
            BaseUrl = configuration["BaseUrl"];
            this.hTTPManagerService = hTTPManagerService;
        }

        [Route("helloworld")]
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World!";
        }

        [Route("test")]
        [HttpGet, MiracleAuthorize]
        public string Test()
        {
            return "test";
        }

        [Route("test2")]
        [HttpGet]
        public async Task<object> Test2()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    //send request
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + "test/helloworld", HttpCompletionOption.ResponseContentRead);
                        return response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (System.Exception ex)
            {
                return ex;
            }
        }

        [Route("test3")]
        [HttpGet]
        public async Task<object> Test3()
        {
            return await hTTPManagerService.GetAsync<object>("test/helloworld");
        }
    }
}