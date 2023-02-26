using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Services.Helpers
{
    /// <summary>
    /// Görsel yükleme silme güncelleme görevlerini üstlenir.
    /// </summary>
    public class ImageManagerService
    {
        public string ProfileImagePath;
        private string newsImagesPath;

        public List<Exception> Exceptions { get; set; }

        public ImageManagerService(IWebHostEnvironment env)
        {
            ProfileImagePath = Path.Combine(env.WebRootPath, "StaticFiles", "Account", "ProfilePhotos");
            if (!Directory.Exists(ProfileImagePath))
                Directory.CreateDirectory(ProfileImagePath);

            newsImagesPath = Path.Combine(env.WebRootPath, "StaticFiles", "News", "Images");
            if (!Directory.Exists(newsImagesPath))
                Directory.CreateDirectory(newsImagesPath);

        }

        public string GetProfileImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return null;

            var file = Path.Combine(ProfileImagePath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/Account/ProfilePhotos/{imageName}";
        }
        public string GetNewsImage(string imageName)
        {
            var file = Path.Combine(newsImagesPath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/News/Images/{imageName}";
        }

        #region Profil Photo
        /// <summary>
        /// Kullanıcının profil görselini kayıt eder.
        /// Eğer kullanıcının zaten bir görseli varsa,
        /// varolan görseli silip yeni görseli kaydeder.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task SaveProfileImage(string imageName, IFormFile file)
        {
            string filePath = Path.Combine(ProfileImagePath, imageName);

            try
            {
                var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await file.CopyToAsync(fileStream);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
        }
        /// <summary>
        /// Kullanıcının görselini günceller
        /// </summary>
        /// <param name="username"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateProfileImage(string oldImageName, string imageName, IFormFile file)
        {
            if (oldImageName != "avatar.png")
                await DeleteProfileImage(oldImageName);

            await SaveProfileImage(imageName, file);
        }
        /// <summary>
        /// Kullanıcının görselini siler.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task DeleteProfileImage(string imageName)
        {
            var profileImage = Path.Combine(ProfileImagePath, imageName);
            if (!File.Exists(profileImage))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(profileImage);
            return Task.CompletedTask;
        }
        #endregion

        #region NewsImage
        /// <summary>
        /// Haberler görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveNewsImage(string imageName, IFormFile file)
        {

            string filePath = Path.Combine(newsImagesPath, imageName);

            try
            {
                var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await file.CopyToAsync(fileStream);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
        }
        /// <summary>
        /// Haberler görselini günceller.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateNewsImage(string oldImageName, string imageName, IFormFile file)
        {
            await DeleteNewsImage(oldImageName);
            await SaveNewsImage(imageName, file);
        }
        /// <summary>
        /// Haberler görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteNewsImage(string imageName)
        {
            var newsImagePath = Path.Combine(newsImagesPath, imageName);
            if (!File.Exists(newsImagePath))
                return Task.FromResult("Image Not Found");


            File.Delete(newsImagePath);
            return Task.CompletedTask;
        }
        #endregion
    }
}
