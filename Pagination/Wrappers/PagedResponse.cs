using System;
using Pagination.DO;

namespace Pagination.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri PreviousPage { get; set; }
        public Uri NextPage { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public PagedResponse(T data, Information information, int pageNumber, int pageSize) 
            : base(data, information)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
