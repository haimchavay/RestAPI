namespace Pagination.DO
{
    public class Information
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public Information()
        {
            Succeeded = false;
            Message = null;
            Error = null;
        }

        public Information(bool succeeded, string message, string error)
        {
            Succeeded = succeeded;
            Message = message;
            Error = error;
        }
    }
}
