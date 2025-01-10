using CoreBankerApi.Domain.Models;
using CoreBankerApi.Domain.Repository;
using CoreBankerApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CoreBankerApi.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChangesAsync();
        }

        public async Task Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByAccountNumber(string accountNumber)
        {
            return await _context.Customers
                .Include(c => c.Industry)
                .ThenInclude(e => e.IndustryTypes)
                .SingleOrDefaultAsync(customer => customer.AccountNumber == accountNumber);
        }

        public async Task<Customer> GetById(string customerId)
        {
            return await _context.Customers
                .SingleOrDefaultAsync(o => o.CustomerId == int.Parse(customerId));
        }

        public async Task Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChangesAsync();
        }
    }
}
