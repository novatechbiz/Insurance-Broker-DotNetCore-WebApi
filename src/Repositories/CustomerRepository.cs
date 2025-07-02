using InsuraNova.Data;
using InsuraNova.Models; // Adjust namespace if different
using Microsoft.EntityFrameworkCore;

namespace InsuraNova.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> SoftDeleteAsync(int id);
    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            customer.IsActive = false;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
