using CoreBankerApi.Domain.Models;

namespace CoreBankerApi.Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> listCustomers();
        Task<Customer> getCustomerByAccountNumber(string AccountNumber);
    }
}
