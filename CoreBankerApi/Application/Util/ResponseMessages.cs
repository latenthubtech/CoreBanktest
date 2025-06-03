namespace CoreBankerApi.Application.Util
{
    public static class ResponseMessages
    {
        public static string GetMessage(ResponseCode responseCode)
        {
            return responseCode switch
            {
                ResponseCode.NotFound => "The requested resource was not found.",
                ResponseCode.InvalidInput => "The provided input is invalid.",
                ResponseCode.Unauthorized => "You are not authorized to perform this action.",
                ResponseCode.DatabaseError => "A database error occurred. Please try again later.",
                ResponseCode.UnknownError => "An unknown error has occurred.",
                _ => "Operation Successful"
            };
        }
    }

}
