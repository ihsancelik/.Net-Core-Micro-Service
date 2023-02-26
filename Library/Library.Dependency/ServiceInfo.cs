using System;

namespace Library.Dependency
{
    public class ServiceInfo
    {
        public Type ServiceInterface { get; set; }
        public Type Service { get; set; }
        public ServiceRegisterType RegisterType { get; set; }

        public ServiceInfo(Type service, ServiceRegisterType registerType)
        {
            Service = service;
            RegisterType = registerType;
        }
        public ServiceInfo(Type serviceInterface, Type service, ServiceRegisterType registerType)
        {
            ServiceInterface = serviceInterface;
            Service = service;
            RegisterType = registerType;
        }
    }
}
