using Miracle.Core.Api.Database.Models;
using System.Collections.Generic;

namespace Miracle.Core.Api.Services
{
    public interface IAppLibService : IBaseResponseService<AppLib>
    {
        public List<AppLib> GetList(bool isActive);
    }
}
