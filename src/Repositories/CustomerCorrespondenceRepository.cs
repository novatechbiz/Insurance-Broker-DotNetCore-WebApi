using InsuraNova.Data;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.EntityFrameworkCore;

namespace InsuraNova.Repositories
{
    public interface ICustomerCorrespondenceRepository : IRepository<CustomerCorrespondence>
    {
        Task<IEnumerable<CustomerCorrespondence>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<CustomerCorrespondence>> GetByCustomerIdWithRelatedEntitiesAsync(int customerId,
            Func<IQueryable<CustomerCorrespondence>, IQueryable<CustomerCorrespondence>> includeRelated);
    }

    public class CustomerCorrespondenceRepository : Repository<CustomerCorrespondence>, ICustomerCorrespondenceRepository
    {
        private readonly AppDbContext _context;

        public CustomerCorrespondenceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerCorrespondence>> GetByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _context.CustomerCorrespondences
                    .Where(cc => cc.CustomerId == customerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to retrieve customer correspondences for customer ID {0}.", customerId), ex);
            }
        }

        public async Task<IEnumerable<CustomerCorrespondence>> GetByCustomerIdWithRelatedEntitiesAsync(int customerId,
            Func<IQueryable<CustomerCorrespondence>, IQueryable<CustomerCorrespondence>> includeRelated)
        {
            try
            {
                IQueryable<CustomerCorrespondence> query = _context.CustomerCorrespondences
                    .Where(cc => cc.CustomerId == customerId);

                query = includeRelated(query);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to retrieve customer correspondences with related entities for customer ID {0}.", customerId), ex);
            }
        }
    }
}