using Microsoft.AspNetCore.WebUtilities;
using System;
using Pagination.Filter;

namespace Pagination.Services
{
    public class UriService : IUriService
    {
        private readonly string baseUri;
        public UriService(string baseUri)
        {
            this.baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = new Uri(string.Concat(baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
