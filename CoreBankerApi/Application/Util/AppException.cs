namespace CoreBankerApi.Application.Util
{
    public class AppException : Exception
    {
        public ResponseCode ErrorCode { get; }

        public AppException(ResponseCode errorCode) : base(ResponseMessages.GetMessage(errorCode))
        {
            ErrorCode = errorCode;
        }
    }

}
