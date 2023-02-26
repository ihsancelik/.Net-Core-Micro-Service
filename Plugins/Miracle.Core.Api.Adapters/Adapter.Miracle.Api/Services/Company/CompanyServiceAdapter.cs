using Library.Responses.Common;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using System.Linq;

namespace Adapter.Miracle.Api.Services
{
    public class CompanyServiceAdapter : ICompanyServiceAdapter
    {
        private readonly MainContext db;
        public CompanyServiceAdapter(MainContext db)
        {
            this.db = db;
        }
        public ListResponse<object> GetUserForTicket()
        {
            var companies = db.Companies
                .Include(s => s.Users)
                .Select(s => new
                {
                    CompanyName = s.Name,
                    Users = s.Users.Select(s => new
                    {
                        Username = s.Username,
                    })
                }).ToList();

            var response = new ListResponse<object>();
            response.SetData(companies);
            return response;
        }
    }
}