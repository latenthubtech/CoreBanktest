namespace CoreBankerApp.Models
{
    public class BaseResponse<T>
    {
        public T data { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
    }
}
