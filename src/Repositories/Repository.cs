using InsuraNova.Data;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.EntityFrameworkCore;

namespace InsuraNova.Repositories
{

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int? userRoleId = null, int? companyId = null);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetWithRelatedEntitiesAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeRelated);


    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(int? userRoleId = null, int? companyId = null)
        {
            try
            {
                if (userRoleId == null || userRoleId == UserRoleIds.Admin)
                {
                    // Fetch all records if userRoleId is null or the user is an admin
                    return await _dbSet.ToListAsync();
                }
                else
                {
                    // Apply filtering based on companyId or another property if role is not admin
                    return await _dbSet.Where(entity => EF.Property<int>(entity, "CompanyId") == companyId).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.EntityListRetrievalFailureMessage, ex);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.EntityRetrievalFailureByIdMessage, id), ex);
            }
        }
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.EntityAdditionFailureMessage, ex);
            }
        }
        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var existingEntity = _context.Set<T>().Local.FirstOrDefault(e =>
            e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(entity.GetType().GetProperty("Id")?.GetValue(entity)) == true);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.EntityUpdateFailureMessage, ex);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.EntityDeletionFailureByIdMessage, id), ex);

            }
        }
        public async Task<T> GetWithRelatedEntitiesAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeRelated)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                query = includeRelated(query);
                return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.EntityRelatedDataRetrievalFailureMessage, id), ex);
            }
        }


    }
}
