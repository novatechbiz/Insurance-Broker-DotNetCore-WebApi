using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> AddRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int id);
    }
    public class RoleService(IRepository<Role> repository) : IRoleService
    {
        private readonly IRepository<Role> _repository = repository;

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveRolesMessage, ex);
            }
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveRoleByIdMessage, ex);
            }
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            try
            {
                return await _repository.AddAsync(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddRoleMessage, ex);
            }
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            try
            {
                return await _repository.UpdateAsync(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateRoleMessage, ex);
            }
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteRoleMessage, ex);
            }
        }
    }
}
