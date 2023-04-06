namespace Azure.CfS.Library.Models
{
    public class Result<T> where T : class
    {
        public Error Error { get; set; } = default!;

        public T Data { get; set; } = default!;

        public Result()
        {
        }
    }

    public class Error
    {
        public int Code { get; set; }

        public string Message { get; set; } = default!;
    }
}
