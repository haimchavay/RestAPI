using Pagination.DO;

namespace Pagination.Wrappers
{
    public class Response<T>
    {
        public T Data { get; set; }
        public Information Information { get; set; }

        public Response(T data, Information information)
        {
            Data = data;
            Information = information;
        }
    }
}
