namespace CoreBankerApi.Application.Util
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public ResponseCode ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

        public static ApiResponse<T> Success(T data) =>
            new() { Data = data, ResponseCode = ResponseCode.SUCCESS, ResponseMessage = ResponseMessages.GetMessage(ResponseCode.SUCCESS) };

        public static ApiResponse<T> Fail(ResponseCode errorCode, string errorMessage = null) =>
            new() { Data = default, ResponseCode = errorCode, ResponseMessage = !String.IsNullOrEmpty(errorMessage) ? errorMessage : ResponseMessages.GetMessage(errorCode) };
    }

}
