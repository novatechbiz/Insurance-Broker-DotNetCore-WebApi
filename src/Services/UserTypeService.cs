using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IUserTypeService
    {
        Task<IEnumerable<UserType>> GetAllUserTypesAsync();
        Task<UserType> GetUserTypeByIdAsync(int id);
        Task<UserType> AddUserTypeAsync(UserType userType);
        Task<UserType> UpdateUserTypeAsync(UserType userType);
        Task<bool> DeleteUserTypeAsync(int id);
    }
    public class UserTypeService(IRepository<UserType> repository) : IUserTypeService
    {
        private readonly IRepository<UserType> _repository = repository;

        public async Task<IEnumerable<UserType>> GetAllUserTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveUserTypesMessage, ex);
            }
        }

        public async Task<UserType> GetUserTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveUserTypeByIdMessage, ex);
            }
        }

        public async Task<UserType> AddUserTypeAsync(UserType userType)
        {
            try
            {
                return await _repository.AddAsync(userType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddUserTypeMessage, ex);
            }
        }

        public async Task<UserType> UpdateUserTypeAsync(UserType userType)
        {
            try
            {
                return await _repository.UpdateAsync(userType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateUserTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteUserTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteUserTypeMessage, ex);
            }
        }
    }
}
