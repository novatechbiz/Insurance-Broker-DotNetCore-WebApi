using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfile>> GetAllUsersAsync();
        Task<UserProfile> GetUserByIdAsync(int id);
        Task<UserProfile> AddUserAsync(UserProfile userProfile);
        Task<UserProfile> UpdateUserAsync(UserProfile userProfile);
        Task<bool> DeleteUserAsync(int id);
        Task<UserProfile?> GetUserByEmailAsync(string email);
        Task<bool> VerifyPassword(string hashedPassword, string enterPassword);
    }
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<UserProfile>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.UserUpdateFailureMessage), ex);
            }
        }

        public async Task<UserProfile?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.UserRetrievalFailureWithIdMessage, id), ex);
            }
        }

        public async Task<UserProfile> AddUserAsync(UserProfile userProfile)
        {
            try
            {
                if (string.IsNullOrEmpty(userProfile.UserPassword))
                {
                    userProfile.UserPassword = PasswordService.HashPassword(DefaultSettings.DefaultSecret); 
                }
                return await _userRepository.AddAsync(userProfile);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.UserAdditionFailureMessage, ex);
            }
        }

        public async Task<UserProfile> UpdateUserAsync(UserProfile userProfile)
        {
            try
            {
                return await _userRepository.UpdateAsync(userProfile);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.UserUpdateFailureMessage, userProfile), ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ApplicationMessages.DeleteUserFailureMessage, id), ex);
            }
        }
        public async Task<UserProfile> GetUserByEmailAsync(string userName)
        {
            try
            {
                return await  _userRepository.FindAsync(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.UserEmailLookupFailureMessage, ex);
            }

        }
        public Task<bool> VerifyPassword(string hashedPassword, string enteredPassword)
        {
            try
            {
                bool isPasswordValid = PasswordService.VerifyPassword(hashedPassword, enteredPassword);
                return Task.FromResult(isPasswordValid);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.PasswordVerificationFailureMessage, ex);

            }
        }


    }
}
