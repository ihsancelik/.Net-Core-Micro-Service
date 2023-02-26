using Library.Helpers.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Package.Manager.Api.Constraints;
using Package.Manager.Api.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Package.Manager.Api.Services
{
    public class PackageService : BaseService
    {
        private readonly DataContext db;

        public PackageService(DataContext db)
        {
            this.db = db;
        }

        public string GetPackageFileUrl(PackageServiceModel model)
        {
            var package = GetPackage(model);
            if (package == null)
            {
                Exception = $"Package:{model.Name} not found!";
                return null;
            }

            if (!File.Exists(package.FilePath))
            {
                Exception = $"Package:{model.Name} not found!";
                return null;
            }

            return GetFilePathUrl(model);
        }

        public List<Database.Package> GetAll()
        {
            return db.Packages.ToList();
        }

        public bool Create(PackageServiceModel model, IFormFile file)
        {
            if (file.Length < 1)
            {
                Exception = $"Package file length not valid. Length:'{file.Length}'";
                return false;
            }

            var fileName = file.FileName;

            var tempDir = Path.Combine(PathConstraints.TEMP, Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var tempFilePath = Path.Combine(tempDir, fileName);

            try
            {
                var exist = PackageIsExist(model);
                if (exist)
                    throw new Exception($"Package:{model.Name} already exist!");

                var hash = string.Empty;
                var filePath = GetFilePath(model);
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                    hash = FileHashGenerator.Generate(fileStream);
                }

                db.Packages.Add(new Database.Package()
                {
                    Platform = model.Platform,
                    Name = model.Name,
                    Version = model.Version,
                    FilePath = filePath,
                    Hash = hash
                });
                DatabaseNumberOfChanges = db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                Directory.Delete(Path.GetDirectoryName(tempFilePath));

                Exception = ex.Message;
                return false;
            }

            Directory.Delete(Path.GetDirectoryName(tempFilePath));

            return true;
        }

        public bool Exist(PackageServiceModel model)
        {
            return PackageIsExist(model);
        }

        public bool Edit(PackageServiceModel model, IFormFile file)
        {
            var deleteResult = Delete(model);
            if (!deleteResult)
                return deleteResult;

            var createResult = Create(model, file);
            return createResult;
        }

        public bool Delete(PackageServiceModel model)
        {
            var package = GetPackage(model);
            if (package == null)
            {
                Exception = $"Package:{model.Name} not found!";
                return false;
            }

            try
            {
                db.Packages.Remove(package);
                DatabaseNumberOfChanges = db.SaveChanges();
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
                return false;
            }

            if (File.Exists(package.FilePath))
                File.Delete(package.FilePath);


            return true;
        }

        public string GetPackageHash(PackageServiceModel model)
        {
            var package = GetPackage(model);
            if (package == null)
            {
                Exception = $"Package:{model.Name} not found!";
                return "";
            }

            return package.Hash;
        }

        private string GetFilePathUrl(PackageServiceModel model)
        {
            var localFilePath = GetFilePath(model);
            var filteredFilePath = localFilePath.Replace(PathConstraints.MPM_Libs, "").Replace("\\", "/");
            var requestPath = StaticFileOptionsConstraints.RequestPath;

            return $"{requestPath}{filteredFilePath}";
        }
        private string GetFilePath(PackageServiceModel model)
        {
            var directory = Path.Combine(PathConstraints.MPM_Libs, model.Platform, Path.GetFileNameWithoutExtension(model.Name), model.Version);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, model.Name);

            if (filePath.Contains("/") || filePath.Contains("\\"))
            {
                var libDir = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(libDir);
            }

            return filePath;
        }

        private Database.Package GetPackage(PackageServiceModel model)
        {
            return db.Packages
                .FirstOrDefault(s => s.Platform == model.Platform && s.Name == model.Name && s.Version == model.Version);
        }
        private bool PackageIsExist(PackageServiceModel model)
        {
            return db.Packages
                .Any(s => s.Platform == model.Platform && s.Name == model.Name && s.Version == model.Version);
        }
    }


    public class PackageServiceModel
    {
        public string Platform { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
