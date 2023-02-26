using Library.Helpers.ExceptionManager;
using Library.Responses.Database;
using Library.Responses.Pagination;
using Miracle.Core.Api.Models.Pagination;
using System.Linq;

namespace Miracle.Core.Api.Services
{
    public interface IBaseService<T> where T : class
    {
        public ExceptionManager ExceptionManager { get; set; }
        public T Get(int id);
        public IQueryable<T> GetList();
        public PagedResponse<T> GetPagedList(PaginationParameterModel paginationModel);
        public DatabaseResponse Create(T model);
        public DatabaseResponse Update(T model);
        public DatabaseResponse Delete(int id);
        public int Count();
    }
}
