using API_Project.Models;
using Microsoft.EntityFrameworkCore;
using API_Project.Repository;
using System;


namespace API_Project.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly BrandContext _context;

        public CustomerRepository(BrandContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddCustomer(Customer customer)
        {
            customer.CustomerId = Guid.NewGuid();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
