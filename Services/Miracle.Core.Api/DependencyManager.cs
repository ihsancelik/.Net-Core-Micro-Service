using Library.Dependency;
using Library.Helpers.Constraints;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miracle.Core.Api.Services;
using Miracle.Core.Api.Services.Helpers;
using Miracle.Core.Api.StaticDatas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Miracle.Core.Api
{
    public class DependencyManager
    {
        private readonly IAppLibService appLibService;
        private readonly ApplicationPartManager applicationPartManager;
        private readonly IServiceCollection services;
        private string currentLibName;

        public bool Success = true;

        public DependencyManager(IAppLibService appLibService, ApplicationPartManager applicationPartManager, IServiceCollection services)
        {
            this.appLibService = appLibService;
            this.applicationPartManager = applicationPartManager;
            this.services = services;
            Initialize();
        }
        public void Inject()
        {
            var appLibList = appLibService.GetList(true);
            if (appLibList == null || appLibList.Count < 1)
                return;

            var libNames = appLibList.Select(s => s.LibName).ToList();
            foreach (string libName in libNames)
            {
                try
                {
                    currentLibName = libName;
                    var libDir = Path.Combine(ApiCorePathConstraints.LibFiles, libName);
                    var jsonPath = Path.Combine(libDir, libName + ".json");
                    var json = File.ReadAllText(jsonPath);
                    var ds = JsonConvert.DeserializeObject<DependencySettings>(json);
                    RegisterAssemblies(libDir, ds);
                }
                catch (Exception ex)
                {
                    AddException(libName, ex);
                }
            }
        }

        private void Initialize()
        {
            var appLibList = appLibService.GetList(false);

            if (appLibList == null || appLibList.Count < 1)
                return;

            appLibList.ForEach(s => s.IsLoaded = false);
            foreach (var appLib in appLibList)
                appLibService.UpdateResponse(appLib);
        }
        private void RegisterAssemblies(string libDir, DependencySettings ds)
        {
            var serviceList = LoadMainAssembly(Path.Combine(libDir, ds.Name));

            foreach (var item in ds.Dependencies)
                LoadAssembly(Path.Combine(libDir, item));

            var appLib = appLibService.GetList().FirstOrDefault(s => s.LibName == ds.Name);
            appLib.IsLoaded = true;
            appLibService.UpdateResponse(appLib);

            RegisterServices(serviceList);
        }
        private void RegisterServices(List<ServiceInfo> serviceList)
        {
            if (serviceList == null || serviceList.Count < 1)
                return;

            // Service register
            foreach (ServiceInfo serviceInfo in serviceList)
            {
                try
                {
                    switch (serviceInfo.RegisterType)
                    {
                        case ServiceRegisterType.Transient:
                            if (serviceInfo.ServiceInterface != null)
                                services.AddTransient(serviceInfo.ServiceInterface, serviceInfo.Service);
                            else
                                services.AddTransient(serviceInfo.Service);
                            continue;

                        case ServiceRegisterType.Scoped:
                            if (serviceInfo.ServiceInterface != null)
                                services.AddScoped(serviceInfo.ServiceInterface, serviceInfo.Service);
                            else
                                services.AddScoped(serviceInfo.Service);
                            continue;

                        case ServiceRegisterType.Singleton:
                            if (serviceInfo.ServiceInterface != null)
                                services.AddSingleton(serviceInfo.ServiceInterface, serviceInfo.Service);
                            else
                                services.AddSingleton(serviceInfo.Service);
                            continue;

                        case ServiceRegisterType.Hosted:
                            if (serviceInfo.ServiceInterface != null)
                            {
                                throw new Exception("Hosted service cannot have a interface!");
                            }
                            else
                            {
                                services.AddTransient(typeof(IHostedService), serviceInfo.Service);
                                continue;
                            }

                        default:
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    AddException(currentLibName, ex);
                    continue;
                }
            }
        }


        private List<ServiceInfo> LoadMainAssembly(string assemblyPath)
        {
            var assembly = LoadAssembly(assemblyPath);
            if (assembly == null)
                return null;

            Type type = assembly.GetTypes().Where(s => s.Name == "DependencyInfo").FirstOrDefault();
            if (type == null)
                return null;

            var dependencyInfoInstance = Activator.CreateInstance(type);
            if (dependencyInfoInstance == null)
                return null;

            List<ServiceInfo> serviceList = null;
            try
            {
                serviceList = (List<ServiceInfo>)dependencyInfoInstance.GetType().GetMethod("GetServices").Invoke(dependencyInfoInstance, null);
                if (serviceList == null || serviceList.Count < 1)
                    return null;
            }
            catch (Exception ex)
            {
                AddException(currentLibName, ex);
                return null;
            }

            applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            return serviceList;
        }
        private Assembly LoadAssembly(string assemblyPath)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                if (assembly == null)
                    return null;
                applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
                return assembly;
            }
            catch (Exception ex)
            {
                AddException(currentLibName, ex);
                return null;
            }
        }

        private void AddException(string libName, Exception exception)
        {
            if (string.IsNullOrEmpty(libName))
                return;
            if (exception == null)
                return;

            if (!StaticDataServerInfo.DependencyExceptions.Any(s => s.LibName == libName))
                StaticDataServerInfo.DependencyExceptions.Add(new DependencyManagerException()
                {
                    LibName = libName,
                    Exceptions = new List<Exception>()
                });

            StaticDataServerInfo.DependencyExceptions
                .Where(s => s.LibName == libName)
                .FirstOrDefault()
                .AddException(exception);

            Success = false;
        }
    }

    public class DependencyManagerException
    {
        public string LibName { get; set; }
        public List<Exception> Exceptions { get; set; }

        public void AddException(Exception exception)
        {
            Exceptions.Add(exception);
        }
    }
}
