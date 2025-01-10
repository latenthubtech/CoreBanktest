using MediatR;
using CoreBankerApi.Domain.Models;

namespace CoreBankerApi.Application.Queries.Customer
{
    public record GetCustomerByAccountNumber(string AccountNumber) : IRequest<CoreBankerApi.Domain.Models.Customer>;
}
