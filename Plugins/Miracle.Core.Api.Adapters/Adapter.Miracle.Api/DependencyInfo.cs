using Adapter.Miracle.Api.Services;
using Library.Dependency;
using System.Collections.Generic;

namespace Adapter.Miracle.Api
{
    public class DependencyInfo
    {
        public static List<ServiceInfo> GetServices()
        {
            return new List<ServiceInfo>()
            {
                new ServiceInfo(typeof(IProductServiceAdapter), typeof(ProductServiceAdapter), ServiceRegisterType.Transient),
                new ServiceInfo(typeof(IUserServiceAdapter), typeof(UserServiceAdapter), ServiceRegisterType.Transient),
                new ServiceInfo(typeof(IRoleServiceAdapter), typeof(RoleServiceAdapter), ServiceRegisterType.Transient),
                new ServiceInfo(typeof(ICompanyServiceAdapter), typeof(CompanyServiceAdapter), ServiceRegisterType.Transient)
            };
        }
    }
}