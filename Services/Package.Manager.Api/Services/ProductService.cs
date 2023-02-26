using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Package.Manager.Api.Services
{
    public class ProductService : BaseService
    {
        private readonly PackageService packageService;

        public ProductInfo ProductInfo { get; private set; }

        public ProductService(PackageService packageService)
        {
            this.packageService = packageService;
        }

        public async Task<bool> Get(string productTag, string platform, string version)
        {
            try
            {
                string filePath = GetFilePath(productTag, platform, version);
                var rawJson = File.ReadAllTextAsync(filePath);
                ProductInfo = JsonSerializer.Deserialize<ProductInfo>(await rawJson);

                return true;
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
                return false;
            }
        }
        public async Task<bool> Create(ProductInfo productInfo)
        {
            try
            {
                for (int i = 0; i < productInfo.Packages.Count; i++)
                {
                    var package = productInfo.Packages[i];


                    var packageServiceModel = new PackageServiceModel()
                    {
                        Platform = productInfo.Platform,
                        Name = package.Name,
                        Version = package.Version,
                    };

                    var hash = packageService.GetPackageHash(packageServiceModel);
                    var url = packageService.GetPackageFileUrl(packageServiceModel);

                    if (hash == string.Empty)
                        throw new Exception(packageService.Exception);

                    if (url == string.Empty)
                        throw new Exception(packageService.Exception);

                    package.Hash = hash;
                    package.Url = url;

                }

                var filePath = GetFilePath(productInfo.ProductTag, productInfo.Platform, productInfo.ProductVersion, true);
                var rawJson = JsonSerializer.Serialize(productInfo);
                await File.WriteAllTextAsync(filePath, rawJson);

                return true;
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
                return false;
            }
        }
        private string GetFilePath(string productTag, string platform, string version, bool createIfNotExistDirectory = false)
        {
            string directory = Path.Combine(Constraints.PathConstraints.MPM_Products, productTag, platform);

            if (createIfNotExistDirectory && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileName = $"{platform}-{productTag}-{version}.json";
            string filePath = Path.Combine(directory, fileName);
            return filePath;
        }
    }

    public class ProductInfo
    {
        public string ProductTag { get; set; }
        public string ProductVersion { get; set; }
        public string Platform { get; set; }
        public List<PackageModel> Packages { get; set; }

        public ProductInfo()
        {
            Packages = new List<PackageModel>();
        }
    }

    public class PackageModel
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Hash { get; set; }
        public string Url { get; set; }
    }
}
