using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;
using System.Net;

namespace InsuraNova.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfile>> GetAllUsersAsync();
        Task<UserProfile> GetUserByIdAsync(int id);
        Task<UserProfile> AddUserAsync(UserProfile userProfile);
        Task<UserProfile> UpdateUserAsync(UserProfile userProfile);
        Task<bool> DeleteUserAsync(int id);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiryTime);
        Task ClearRefreshTokenAsync(int userId);
        Task<UserProfile?> GetUserByEmailAsync(string email);
        Task<UserProfile> GetUserByResetTokenAsync(string resetToken);
        Task<bool> VerifyPassword(string hashedPassword, string enterPassword);
        Task SaveResetTokenAsync(int userId, string resetToken, DateTime resetTokenExpiryTime);
        Task PasswordReset(int userId, string newPassword);
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
                var existingUser = await _userRepository.GetUserByUserNameAsync(userProfile.UserName.Trim());
                if (existingUser != null)
                {
                    throw new Exception($"A user with username '{userProfile.UserName}' already exists.");
                }

                userProfile.UserPassword = PasswordService.HashPassword(userProfile.UserPassword);
                return await _userRepository.AddAsync(userProfile);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(ApplicationMessages.UserAlreadyExistsMessage, ex);
                }
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
        public async Task<UserProfile> GetUserByEmailAsync(string email)
        {
            try
            {
                return await  _userRepository.FindAsync(u => u.Email == email);
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

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new Exception($"User with ID {userId} not found.");

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update refresh token.", ex);
            }
        }

        public async Task ClearRefreshTokenAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userRepository.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update refresh token.", ex);
            }
        }

        public Task SaveResetTokenAsync(int userId, string resetToken, DateTime resetTokenExpiryTime)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(userId).Result;
                if (user == null)
                {
                    throw new Exception($"User with ID {userId} not found.");
                }
                    
                user.ResetToken = resetToken;
                user.ResetTokenExpiryTime = resetTokenExpiryTime;
                return _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save reset token.", ex);
            }
        }

        public Task<UserProfile> GetUserByResetTokenAsync(string resetToken)
        {
            try
            {
                var encodedToken = WebUtility.UrlEncode(resetToken);

                var user = _userRepository.FindAsync(u => u.ResetToken == encodedToken && u.ResetTokenExpiryTime > DateTime.UtcNow);
                if (user == null)
                {
                    throw new Exception(ApplicationMessages.UserResetTokenNotFoundMessage);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.UserResetTokenLookupFailureMessage, ex);
            }
        }

        public Task PasswordReset(int userId, string newPassword)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(userId).Result;
                if (user == null)
                {
                    throw new Exception($"User with ID {userId} not found.");
                }
                user.UserPassword = newPassword;
                user.ResetToken = null; // Clear reset token after successful password reset
                user.ResetTokenExpiryTime = null; // Clear reset token expiry time

                return _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.PasswordResetFailureMessage, ex);

            }
        }
    }
}
