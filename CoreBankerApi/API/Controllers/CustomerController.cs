using CoreBankerApi.API.Responses;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CoreBankerApi.Application.Queries.Customer;

namespace CoreBankerApi.API.Controllers
{
    [ApiController]
    [Route("/api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetCustomerByAccountNumber(string accountNumber)
        {
            var customers = await _mediator.Send(new GetCustomerByAccountNumber(accountNumber));
            return Ok(new BaseResponse<dynamic> { Data = customers, Message = "Application Successful", StatusCode = HttpStatusCode.OK, Success = true });
        }
    }
}
