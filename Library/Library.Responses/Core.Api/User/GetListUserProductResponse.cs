using Library.Responses.Common;
using Library.Responses.Database;
using System;
using System.Collections.Generic;

namespace Library.Responses.Core.Api.User
{
    public class GetListUserProductResponse<USER_PRODUCT, PRODUCT> : BaseResponse 
        where PRODUCT : class
        where USER_PRODUCT : class
    {
        public IEnumerable<USER_PRODUCT> UserProducts { get; set; }
        public IEnumerable<PRODUCT> Products { get; set; }
        public GetListUserProductResponse() : base()
        {
        }
        public GetListUserProductResponse(bool success) : base(success)
        {
        }
        public GetListUserProductResponse(DatabaseResponse dbResult) : base(dbResult)
        {
        }
        public GetListUserProductResponse(Exception exception, bool success = false) : base(exception, success)
        {
        }
        public GetListUserProductResponse(string exceptionMessage, bool success = false) : base(exceptionMessage, success)
        {
        }
        public GetListUserProductResponse(IEnumerable<Exception> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public GetListUserProductResponse(IEnumerable<string> exceptionMessages, bool success = false) : base(exceptionMessages, success)
        {
        }
        public void SetData(IEnumerable<PRODUCT> products, IEnumerable<USER_PRODUCT> userProducts)
        {
            Products = products;
            UserProducts = userProducts;
        }
    }
}