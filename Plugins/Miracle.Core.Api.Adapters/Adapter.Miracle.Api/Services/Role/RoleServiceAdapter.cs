using Library.Responses.Common;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using System.Linq;

namespace Adapter.Miracle.Api.Services
{
    public class RoleServiceAdapter : IRoleServiceAdapter
    {
        private readonly MainContext db;
        public RoleServiceAdapter(MainContext db)
        {
            this.db = db;
        }

        public ListResponse<string> GetByUsername(string username)
        {
            var roles = db.Users
                    .Where(s => s.Username == username)
                    .Include(s => s.User_Roles)
                    .ThenInclude(s => s.Role)
                    ?.Select(s => s.User_Roles)
                    ?.FirstOrDefault()
                    ?.Select(s => s.Role.Value);

            if (roles == null || roles.Count() == 0)
                return new ListResponse<string>("Unauthorized");

            var response = new ListResponse<string>();
            response.SetData(roles);
            return response;
        }
    }
}
