namespace CoreBankerApi.Application.Util
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred.");

            var errorCode = ResponseCode.UnknownError;
            var response = ApiResponse<string>.Fail(errorCode);

            //Manage Response Context
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;


            switch (exception)
            {
                case AppException appException:

                    errorCode = ((AppException)exception).ErrorCode;
                    response = ApiResponse<string>.Fail(errorCode);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    break;

                default:

                    response = ApiResponse<string>.Fail(errorCode, exception.Message);

                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);
            return true;
        }
    }

}
