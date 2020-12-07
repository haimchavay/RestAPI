namespace Pagination.Filter
{
    public class PaginationFilter
    {
        private const int MIN_PAGE_SIZE = 1;
        private const int MIN_PAGE_NUMBER = 1;
        private const int MAX_PAGE_SIZE = 3;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            PageNumber = MIN_PAGE_NUMBER;
            PageSize = MAX_PAGE_SIZE;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber > MIN_PAGE_NUMBER ? pageNumber : MIN_PAGE_NUMBER;
            PageSize = pageSize >= MIN_PAGE_SIZE && pageSize < MAX_PAGE_SIZE ? pageSize : MAX_PAGE_SIZE;
        }
    }
}
