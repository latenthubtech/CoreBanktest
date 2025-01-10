using System.Net;

namespace CoreBankerApi.API.Responses
{
    public class BaseResponse<T>
    {
        //public BaseResponse(bool Success, string Message, HttpStatusCode StatusCode, T Data)
        //{
        //this.Success = Success;
        //this.Message = Message;
        //this.StatusCode = StatusCode;
        //this.Data = Data;
        //}
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }

    }
}
