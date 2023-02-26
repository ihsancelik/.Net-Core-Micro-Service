using Miracle.Api.Database.Models;
using Miracle.Api.Models;
using Miracle.Api.Models.Helpers;
using Miracle.Api.Responses.Common;
using Miracle.Api.Services.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public class UserService : IUserService
    {
        private readonly HTTPManagerService httpManagerService;
        public UserService(HTTPManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService;
        }

        public async Task<GetResponse<User>> Get(string authToken)
        {
            return await httpManagerService.GetAsync<GetResponse<User>>($"user/getoutsource", authToken);
        }
        public async Task<GetResponse<User>> GetById(int userId, string authToken)
        {
            return await httpManagerService.GetAsync<GetResponse<User>>($"user/get/{userId}", authToken);
        }

        #region User
        public async Task<GetResponse<string>> GetUserImageAsync(string authToken)
        {
            return await httpManagerService.GetAsync<GetResponse<string>>($"user/getProfileImageOutSource", authToken);
        }
        public async Task<EmptyResponse> UpdateUserOutSourceAsync(UserOutSourceModel model, string authToken)
        {
            var multipartContent = new MultipartFormDataContent();

            if (model.ProfilePhoto != null)
                using (var stream = model.ProfilePhoto.OpenReadStream())
                {
                    var imageBytes = new byte[stream.Length];
                    stream.Read(imageBytes);
                    multipartContent.Add(new ByteArrayContent(imageBytes), "ProfilePhoto", model.ProfilePhoto.FileName);
                }

            multipartContent.Add(new StringContent(model.Name.ToString()), "Name");
            multipartContent.Add(new StringContent(model.Surname.ToString()), "Surname");
            multipartContent.Add(new StringContent(model.Username.ToString()), "Username");
            multipartContent.Add(new StringContent(model.Email.ToString()), "Email");
            multipartContent.Add(new StringContent(model.PhoneNumber.ToString()), "PhoneNumber");

            return await httpManagerService.PutAsync<EmptyResponse>("user/updateoutsource", multipartContent, authToken);
        }
        #endregion

        #region UnUsed
        public PagedListResponse<Product> GetProducts(int userId, PaginationParameterModel model)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}