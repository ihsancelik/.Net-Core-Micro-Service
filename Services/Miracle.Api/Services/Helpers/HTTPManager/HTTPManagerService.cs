using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Miracle.Api.Services.Helpers
{
    public class HTTPManagerService
    {
        private string BaseUrl;
        private const string ApplicationJson = "application/json";
        public HTTPManagerService(IConfiguration configuration)
        {
            BaseUrl = configuration["BaseUrl"];
        }
        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public async Task<ReturnModel> GetAsync<ReturnModel>(string url, string authToken = "") where ReturnModel : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.DefaultRequestHeaders.Clear();

                    if (!string.IsNullOrEmpty(authToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                    httpClient.DefaultRequestHeaders.Add("Authentication-Type", "Web");

                    var httpResponse = await httpClient.GetAsync(BaseUrl + url);
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnModel>(responseContent);
                }
            }
        }

        public async Task<ReturnModel> PostAsync<Model, ReturnModel>(string url, Model model, string authToken = "")
            where Model : class
            where ReturnModel : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(authToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                    httpClient.DefaultRequestHeaders.Add("Authentication-Type", "Web");

                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, ApplicationJson);

                    var httpResponse = await httpClient.PostAsync(BaseUrl + url, content);
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnModel>(responseContent);
                }
            }
        }
        public async Task<ReturnModel> PostAsync<ReturnModel>(string url, MultipartFormDataContent model, string authToken = "")
            where ReturnModel : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(authToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                    httpClient.DefaultRequestHeaders.Add("Authentication-Type", "Web");

                    var httpResponse = await httpClient.PutAsync(BaseUrl + url, model);
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnModel>(responseContent);
                }
            }
        }

        public async Task<ReturnModel> PutAsync<Model, ReturnModel>(string url, Model model, string authToken = "")
            where Model : class
            where ReturnModel : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(authToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                    httpClient.DefaultRequestHeaders.Add("Authentication-Type", "Web");

                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, ApplicationJson);

                    var httpResponse = await httpClient.PutAsync(BaseUrl + url, content);
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnModel>(responseContent);
                }
            }
        }
        public async Task<ReturnModel> PutAsync<ReturnModel>(string url, MultipartFormDataContent model, string authToken = "")
            where ReturnModel : class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                httpClientHandler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(authToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                    httpClient.DefaultRequestHeaders.Add("Authentication-Type", "Web");

                    var httpResponse = await httpClient.PutAsync(BaseUrl + url, model);
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReturnModel>(responseContent);
                }
            }
        }
    }
}