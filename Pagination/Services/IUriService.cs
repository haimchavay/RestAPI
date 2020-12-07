using System;
using Pagination.Filter;

namespace Pagination.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
