using CoreBankerApi.Domain.Models;

namespace CoreBankerApi.Domain.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> GetById(string customerId);
        Task<Customer> GetByAccountNumber(string accountNumber);
        Task<IEnumerable<Customer>> GetAll();
        Task Add(Customer customer);
        Task Update(Customer customer);
        Task Delete(Customer customer);
    }
}
