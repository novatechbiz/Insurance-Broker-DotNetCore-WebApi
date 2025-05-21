using InsuraNova.Data;
using InsuraNova.Resources;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InsuraNova.Repositories
{
    public interface IUserRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetUserByUserNameAsync(string userName);
        Task<UserProfile> FindAsync(Expression<Func<UserProfile, bool>> predicate);

    }

    public class UserRepository(AppDbContext context) : Repository<UserProfile>(context), IUserRepository
    {
        private readonly AppDbContext _context = context;

      
        public async Task<UserProfile> GetUserByUserNameAsync(string userName)
        {
            try
            {
                return await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.UserRetrievalFailureByEmailMessage, userName), ex);
            }
        }
        public async Task<UserProfile> FindAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            try
            {
                return await _context.UserProfiles.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.UserFindFailureMessage, ex);
            }
        }

    }
}
