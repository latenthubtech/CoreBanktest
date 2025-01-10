using CoreBankerApi.Application.Queries.Customer;
using CoreBankerApi.Application.Services;
using CoreBankerApi.Domain.Models;
using MediatR;

namespace CoreBankerApi.Application.Handlers
{
    public class GetCustomersHandler : IRequestHandler<GetCustomerByAccountNumber, Customer>
    {
        private readonly ICustomerService _customerService;
        public GetCustomersHandler(ICustomerService customerService) { 
            _customerService = customerService;
        }
        public Task<Customer> Handle(GetCustomerByAccountNumber request, CancellationToken cancellationToken)
        {
            return _customerService.getCustomerByAccountNumber(request.AccountNumber);
        }
    }
}
