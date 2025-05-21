using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
  

    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
    }
    public class UserRoleService(IRepository<UserRole> repository) : IUserRoleService
    {
        private readonly IRepository<UserRole> _repository = repository;

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveUserRolesMessage, ex);
            }
        }
    }
}
