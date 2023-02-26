namespace Miracle.Core.Api.Models.Pagination
{
    public class PaginationParameterModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string PropertyName { get; set; }
        public string FilterType { get; set; }
        public string SearchValue { get; set; }
    }
}
