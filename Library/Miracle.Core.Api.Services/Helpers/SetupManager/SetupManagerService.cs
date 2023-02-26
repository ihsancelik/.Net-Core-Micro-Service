using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Services.Helpers
{
    public class SetupManagerService
    {
        private string productsPath;
        private string pluginsPath;

        public SetupManagerService(IWebHostEnvironment env)
        {
            productsPath = Path.Combine(env.WebRootPath, "Setups", "Products");
            pluginsPath = Path.Combine(env.WebRootPath, "Setups", "Plugins");

            if (!Directory.Exists(productsPath))
                Directory.CreateDirectory(productsPath);

            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);
        }

        public Task Save(IFormFile file, string fileName, bool isProduct)
        {
            var filePath = string.Empty;

            if (isProduct)
                filePath = Path.Combine(productsPath, fileName);
            else
                filePath = Path.Combine(pluginsPath, fileName);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex);
            }
        }

        public Task Delete(string fileName, bool isProduct)
        {
            var filePath = string.Empty;

            if (isProduct)
                filePath = Path.Combine(productsPath, fileName);
            else
                filePath = Path.Combine(pluginsPath, fileName);

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex);
            }

            return Task.FromResult(true);
        }

        public string GetPath(bool isProduct)
        {
            return isProduct ? productsPath : pluginsPath;
        }
    }
}
