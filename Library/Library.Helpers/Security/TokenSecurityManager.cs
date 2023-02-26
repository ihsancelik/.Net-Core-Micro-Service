using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library.Helpers.Security
{
    // Bu kısmı yapıp yapmayacağıma emin değilim. 
    // Gerçekten böyle bir şeye ihtiyaç var mı?
    // Bu class localde auth.api ile haberleşip token veya userın
    // Geçerli olup olmadığını kontrol ediyor.
    public class TokenSecurityManager
    {
        private HttpClient client;

        private string tokenIsValidUrl;
        private string userIsValidUrl;
        public TokenSecurityManager(IConfiguration configuration)
        {
            client = new HttpClient();

            var authUrl = configuration.GetSection("AuthApi:AuthUrl").Value;
            var tokenIsValid = configuration.GetSection("AuthApi:TokenIsValid").Value;
            var userIsValid = configuration.GetSection("AuthApi:UserIsValid").Value;

            tokenIsValidUrl = $"{authUrl}/{tokenIsValid}";
            userIsValidUrl = $"{authUrl}/{userIsValid}";
        }

        public async Task<bool> TokenIsValidAsync()
        {
            var response = await client.GetAsync(tokenIsValidUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Auth api not responding!");

            var result = await response.Content.ReadAsStringAsync();

            return bool.Parse(result);
        }
        public async Task<bool> UserIsValidAsync()
        {
            var response = await client.GetAsync(userIsValidUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Auth api not responding!");

            var result = await response.Content.ReadAsStringAsync();

            return bool.Parse(result);
        }
    }
}
