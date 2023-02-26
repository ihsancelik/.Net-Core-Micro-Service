using Library.Helpers.Constraints;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Services.Helpers
{
    public class AppLibManager
    {
        public async Task SaveAsync(string libraryName, IFormFile libFile)
        {
            var tempDir = ApiCorePathConstraints.Temp;
            var dependencyLibDir = Path.Combine(ApiCorePathConstraints.LibFiles, libraryName);
            var appCurrentDir = ApiCorePathConstraints.Current;

            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(tempDir, guid);
            var zipName = libFile.FileName;
            var zipPath = Path.Combine(tempPath, zipName);
            try
            {
                Directory.CreateDirectory(tempPath);
                if (!Directory.Exists(dependencyLibDir))
                    Directory.CreateDirectory(dependencyLibDir);
            }
            catch
            {
                return;
            }
            var fileStream = new FileStream(zipPath, FileMode.OpenOrCreate);
            await libFile.CopyToAsync(fileStream);
            fileStream.Close();

            ZipFile.ExtractToDirectory(zipPath, tempPath);

            var settingsDir = Path.Combine(tempPath, "Settings");
            var libraryDir = Path.Combine(tempPath, "Library");
            var dependenciesDir = Path.Combine(tempPath, "Dependencies");
            if (!Directory.Exists(settingsDir) || !Directory.Exists(libraryDir) || !Directory.Exists(dependenciesDir))
            {
                Directory.Delete(tempPath, true);
                return;
            }

            var settingsName = libraryName + ".json";
            var settingsPath = Path.Combine(settingsDir, settingsName);
            if (!File.Exists(settingsPath))
            {
                Directory.Delete(tempPath, true);
                return;
            }

            string json = null;
            try
            {
                json = File.ReadAllText(settingsPath);
            }
            catch
            {
                Directory.Delete(tempPath, true);
                return;
            }

            if (string.IsNullOrEmpty(json))
            {
                Directory.Delete(tempPath, true);
                return;
            }

            var dependencySettings = JsonConvert.DeserializeObject<DependencySettings>(json);
            if (dependencySettings == null)
            {
                Directory.Delete(tempPath, true);
                return;
            }

            if (dependencySettings.Name != libraryName)
            {
                Directory.Delete(tempPath, true);
                return;
            }

            var libraryPath = Path.Combine(libraryDir, libraryName);
            if (!File.Exists(libraryPath))
            {
                Directory.Delete(tempPath, true);
                return;
            }

            foreach (string dependencyName in dependencySettings.Dependencies)
            {
                var currentDependencyPath = Path.Combine(appCurrentDir, dependencyName);
                var newDependencyPath = Path.Combine(dependenciesDir, dependencyName);
                if (File.Exists(currentDependencyPath))
                {
                    Directory.Delete(tempPath, true);
                    return;
                }
                if (!File.Exists(newDependencyPath))
                {
                    Directory.Delete(tempPath, true);
                    return;
                }
            }

            File.Copy(settingsPath, Path.Combine(dependencyLibDir, settingsName));
            File.Copy(libraryPath, Path.Combine(dependencyLibDir, libraryName));
            foreach (string dependencyName in dependencySettings.Dependencies)
            {
                var newDependencyPath = Path.Combine(dependenciesDir, dependencyName);
                File.Copy(newDependencyPath, Path.Combine(dependencyLibDir, dependencyName));
            }

            Directory.Delete(tempPath, true);
        }

        public void Delete(string libName)
        {
            try
            {
                var libPath = Path.Combine(ApiCorePathConstraints.LibFiles, libName);
                if (Directory.Exists(libPath))
                    Directory.Delete(libPath, true);
            }
            catch
            {

            }
        }
    }
    public class DependencySettings
    {
        public string Name { get; set; }
        public string[] Dependencies { get; set; }
    }
}
