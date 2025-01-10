using CoreBankerApi.Domain.Models;
using CoreBankerApi.Domain.Repository;

namespace CoreBankerApi.Application.Services.Impl
{
    public class CustomerService: ICustomerService
    {
        ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository) {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> getCustomerByAccountNumber(string AccountNumber)
        {
            return await _customerRepository.GetByAccountNumber(AccountNumber);
        }

        public async Task<IEnumerable<Customer>> listCustomers()
        {
            return await _customerRepository.GetAll();
        }
    }
}
