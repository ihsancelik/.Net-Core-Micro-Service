using Miracle.Api.Models.Helpers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Miracle.Api.Extensions
{
    public static class PaginationExtension
    {
        private static int maxPageSize = 100;
        public static Database.Pagination.PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         PaginationParameterModel model) where T : class
        {
            int page = model.Page;
            int pageSize = model.PageSize;
            string propertyName = model.PropertyName;
            string filterType = model.FilterType;
            string searchFilter = model.SearchValue;

            if (page < 1)
                page = 1;

            if (pageSize < 0)
                pageSize = maxPageSize;

            if (pageSize < 1)
                pageSize = 1;

            if (pageSize > maxPageSize)
                pageSize = maxPageSize;

            var result = new Database.Pagination.PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;


            if (string.IsNullOrEmpty(filterType) || string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(searchFilter))
            {
                result.List = query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.List = query
                     .Where($"{propertyName}.{filterType}(\"{searchFilter}\")")
                     .Skip(skip)
                     .Take(pageSize)
                     .ToList();
            }

            return result;
        }
    }
}
