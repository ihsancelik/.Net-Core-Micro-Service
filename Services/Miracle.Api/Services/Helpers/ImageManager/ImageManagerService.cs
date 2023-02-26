using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Miracle.Api.Services.Helpers
{
    /// <summary>
    /// Görsel yükleme silme güncelleme görevlerini üstlenir.
    /// </summary>
    public class ImageManagerService
    {
        private string AboutImagesPath;
        private string NewsImagesPath;
        private string SliderImagesPath;
        private string ProductImagesPath;
        private string ProfileImagePath;
        private string TicketImagePath;
        public List<Exception> Exceptions { get; set; }

        public ImageManagerService(IWebHostEnvironment env)
        {
            AboutImagesPath = Path.Combine(env.WebRootPath, "StaticFiles", "About");
            if (!Directory.Exists(AboutImagesPath))
                Directory.CreateDirectory(AboutImagesPath);

            NewsImagesPath = Path.Combine(env.WebRootPath, "StaticFiles", "News", "Images");
            if (!Directory.Exists(NewsImagesPath))
                Directory.CreateDirectory(NewsImagesPath);

            SliderImagesPath = Path.Combine(env.WebRootPath, "StaticFiles", "Slider", "Images");
            if (!Directory.Exists(SliderImagesPath))
                Directory.CreateDirectory(SliderImagesPath);

            ProductImagesPath = Path.Combine(env.WebRootPath, "StaticFiles", "Product", "Images");
            if (!Directory.Exists(ProductImagesPath))
                Directory.CreateDirectory(ProductImagesPath);

            ProfileImagePath = Path.Combine(env.WebRootPath, "StaticFiles", "Account", "ProfilePhotos");
            if (!Directory.Exists(ProfileImagePath))
                Directory.CreateDirectory(ProfileImagePath);

            TicketImagePath = Path.Combine(env.WebRootPath, "StaticFiles", "Tickets");
            if (!Directory.Exists(TicketImagePath))
                Directory.CreateDirectory(TicketImagePath);

        }

        public string GetAboutImage(string imageName)
        {
            var file = Path.Combine(AboutImagesPath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/About/{imageName}";
        }
        public string GetNewsImage(string imageName)
        {
            var file = Path.Combine(NewsImagesPath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/News/Images/{imageName}";
        }
        public string GetSliderImage(string imageName)
        {
            var file = Path.Combine(SliderImagesPath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/Slider/Images/{imageName}";
        }
        public string GetProductImage(string imageName)
        {
            var file = Path.Combine(ProductImagesPath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/Product/Images/{imageName}";
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
        public string GetTicketImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return null;

            var file = Path.Combine(TicketImagePath, imageName);

            if (!File.Exists(file))
                return null;

            return $"Files/Tickets/{imageName}";
        }


        #region About
        /// <summary>
        /// About görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveAboutImage(string imageName, IFormFile file)
        {
            string filePath = Path.Combine(AboutImagesPath, imageName);

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
        /// About görselini günceller.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 
        public async Task UpdateAboutImage(string oldImageName, string imageName, IFormFile file)
        {
            DeleteAboutImage(oldImageName);
            await SaveAboutImage(imageName, file);
        }
        /// <summary>
        /// About görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteAboutImage(string imageName)
        {
            var aboutImagePath = Path.Combine(AboutImagesPath, imageName);
            if (!File.Exists(aboutImagePath))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(aboutImagePath);
            return Task.CompletedTask;
        }

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
            DeleteProfileImage(oldImageName);
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

        #region News
        /// <summary>
        /// Haberler görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveNewsImage(string imageName, IFormFile file)
        {
            string filePath = Path.Combine(NewsImagesPath, imageName);

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
            DeleteNewsImage(oldImageName);
            await SaveNewsImage(imageName, file);
        }
        /// <summary>
        /// Haberler görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteNewsImage(string imageName)
        {
            var newsImagePath = Path.Combine(NewsImagesPath, imageName);
            if (!File.Exists(newsImagePath))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(newsImagePath);
            return Task.CompletedTask;
        }
        #endregion

        #region Slider
        /// <summary>
        /// Slider görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveSliderImage(string imageName, IFormFile file)
        {

            string filePath = Path.Combine(SliderImagesPath, imageName);

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
        /// Slider görselini günceller.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateSliderImage(string oldImageName, string imageName, IFormFile file)
        {
            DeleteSliderImage(oldImageName);
            await SaveSliderImage(imageName, file);
        }
        /// <summary>
        /// Slider görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteSliderImage(string imageName)
        {
            var sliderImagePath = Path.Combine(SliderImagesPath, imageName);
            if (!File.Exists(sliderImagePath))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(sliderImagePath);
            return Task.CompletedTask;
        }
        #endregion

        #region ProductImage
        /// <summary>
        /// Product görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveProductImage(string imageName, IFormFile file)
        {
            string filePath = Path.Combine(ProductImagesPath, imageName);

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
        /// Product görselini günceller.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateProductImage(string oldImageName, string imageName, IFormFile file)
        {
            DeleteProductImage(oldImageName);
            await SaveProductImage(imageName, file);
        }

        /// <summary>
        /// Product görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteProductImage(string imageName)
        {
            var productImagePath = Path.Combine(ProductImagesPath, imageName);
            if (!File.Exists(productImagePath))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(productImagePath);
            return Task.CompletedTask;
        }
        #endregion

        #region Ticket
        /// <summary>
        /// Support görselini kaydeder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task SaveTicketImage(string imageName, IFormFile file)
        {
            string filePath = Path.Combine(TicketImagePath, imageName);

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
        /// Support görselini günceller.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateTicketImage(string oldImage, string imageName, IFormFile file)
        {
            DeleteTicketImage(oldImage);
            await SaveTicketImage(imageName, file);
        }


        /// <summary>
        /// Support görselini siler.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public Task DeleteTicketImage(string imageName)
        {
            var ticketImagePath = Path.Combine(TicketImagePath, imageName);
            if (!File.Exists(ticketImagePath))
            {
                return Task.FromResult("Image Not Found");
            }

            File.Delete(ticketImagePath);
            return Task.CompletedTask;
        }
        #endregion
    }
}