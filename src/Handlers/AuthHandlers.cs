using AutoMapper;
using InsuraNova.Dto;
using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace InsuraNova.Handlers
{
    // Commands
    public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;
    public record LogoutCommand : IRequest<LogOutResponse>;
    public record RefreshTokenCommand : IRequest<LoginResponse>;
    public record ForgotPasswordCommand(string Email, string Type) : IRequest<Unit>;
    public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Unit>;

    // Handlers
    public class LoginHandler(IUserRepository userRepository, IUserService userService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<LoginHandler> logger)
        : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<LoginHandler> _logger = logger;

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var loginRequest = request.LoginRequest;
                _logger.LogInformation("Login attempt with email: {Username}", loginRequest.Username);

                // Check if the user exists by email
                var user = await _userRepository.GetUserByUserNameAsync(loginRequest.Username.Trim());
                if (user == null)
                {
                    _logger.LogWarning("User not found Username: {Username}", loginRequest.Username.Trim());
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Username does not exist."
                    };
                }

                // Validate the password
                var isValidPassword = await _userService.VerifyPassword(loginRequest.Password.Trim(), user.UserPassword);
                if (!isValidPassword)
                {
                    _logger.LogWarning("Incorrect password for username: {Username}", loginRequest.Username.Trim());
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Password is incorrect."
                    };
                }

                // Generate tokens
                var token = TokenHelper.GenerateToken(user);
                var refreshToken = TokenHelper.GenerateRefreshToken(user);

                // Store refresh token in the database or override the existing refresh token and set expiry time according to RememberMe option
                var refreshTokenExpiry = loginRequest.RememberMe
                    ? DateTime.UtcNow.AddDays(30)
                    : DateTime.UtcNow.AddHours(2);

                await _userService.UpdateRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

                // Set refresh token cookie
                TokenHelper.SetRefreshTokenCookie(_httpContextAccessor.HttpContext.Response, refreshToken, refreshTokenExpiry);

                // Map the user to the UserDto
                var userDto = _mapper.Map<UserDto>(user);

                var response = new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = userDto,
                };

                _logger.LogInformation("Login successful for username: {Username}", loginRequest.Username.Trim());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing login.");
                throw new Exception("An error occurred during login. See inner exception for details.", ex);
            }
        }
    }

    public class LogoutHandler(IUserService userService, IHttpContextAccessor httpContextAccessor, ITokenBlacklistService tokenBlacklistService, ILogger<LogoutHandler> logger)
        : IRequestHandler<LogoutCommand, LogOutResponse>
    {
        private readonly IUserService _userService = userService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ITokenBlacklistService _tokenBlacklistService = tokenBlacklistService;
        private readonly ILogger<LogoutHandler> _logger = logger;

        public async Task<LogOutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
                var jtiClaim = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                var expClaim = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(jtiClaim) || string.IsNullOrEmpty(expClaim))
                    return new LogOutResponse { Success = false, Message = "Unauthorized" };

                int userId = int.Parse(userIdClaim);
                var expUnix = long.Parse(expClaim);
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

                // Clear the refresh token from the database
                await _userService.ClearRefreshTokenAsync(userId);

                // Clear the refresh token cookie
                TokenHelper.ClearRefreshTokenCookie(_httpContextAccessor.HttpContext.Response);

                // Blacklist the JWT token
                await _tokenBlacklistService.BlacklistTokenAsync(jtiClaim, expiryDate);

                _logger.LogInformation("Logout successful for user ID: {UserId}", userId);

                return new LogOutResponse
                {
                    Success = true,
                    Message = "Logout successful."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing logout.");
                return new LogOutResponse
                {
                    Success = false,
                    Message = "An error occurred during logout. See inner exception for details."
                };
            }
        }
    }

    public class RefreshTokenHandler(IUserService userService, IHttpContextAccessor httpContextAccessor, ITokenBlacklistService tokenBlacklistService, IMapper mapper, ILogger<LogoutHandler> logger)
        : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IUserService _userService = userService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ITokenBlacklistService _tokenBlacklistService = tokenBlacklistService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<LogoutHandler> _logger = logger;

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var refreshToken = TokenHelper.GetRefreshTokenFromCookie(httpContext.Request);

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return new LoginResponse { Success = false, Message = "Refresh token not found." };
                }

                // Validate refresh token format and expiry
                if (!TokenHelper.ValidateToken(refreshToken, out var principal))
                {
                    return new LoginResponse { Success = false, Message = "Invalid refresh token." };
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new LoginResponse { Success = false, Message = "Invalid token claims." };
                }

                var user = await _userService.GetUserByIdAsync(int.Parse(userId));
                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return new LoginResponse { Success = false, Message = "Refresh token expired or invalid." };
                }

                // Blacklist current access token if present
                var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                if (TokenHelper.ValidateToken(accessToken, out var accessPrincipal))
                {
                    var jti = accessPrincipal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                    var expClaim = accessPrincipal.FindFirstValue(JwtRegisteredClaimNames.Exp);

                    if (!string.IsNullOrEmpty(jti) && long.TryParse(expClaim, out long expSeconds))
                    {
                        var expTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                        await _tokenBlacklistService.BlacklistTokenAsync(jti, expTime);
                    }
                }

                // Issue new tokens
                var newAccessToken = TokenHelper.GenerateToken(user);

                return new LoginResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Token = newAccessToken,
                    User = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refreshing token.");
                throw new Exception("An error occurred during token refresh. See inner exception for details.", ex);
            }
        }

    }

    public class ForgotPasswordHandler(IUserService userService, IEmailService emailService, ILogger<ForgotPasswordHandler> logger)
        : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IUserService _userService = userService;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<ForgotPasswordHandler> _logger = logger;
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Forgot password request for unknown email: {Email}", request.Email);
                return Unit.Value; // Avoid revealing whether email exists
            }

            var token = WebUtility.UrlEncode(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));

            var resetLink = "";
            if (request.Type == ApplicationTypes.Web)
            {
                resetLink = $"http://localhost:3000/reset-password?token={token}";
            }
            else if (request.Type == ApplicationTypes.Mobile)
            {
                resetLink = $"{request.Type}://reset-password?token={token}";
            }
            else
            {
                _logger.LogWarning("Unsupported type for forgot password: {Type}", request.Type);
                return Unit.Value;
            }

            try
            {
                await _emailService.SendResetPasswordEmailAsync(user.Email, resetLink);

                // Save the reset token in the database
                var resetTokenExpiry = DateTime.UtcNow.AddMinutes(10);
                await _userService.SaveResetTokenAsync(user.Id, token, resetTokenExpiry);
                _logger.LogInformation("Reset password email sent to: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending reset password email to {Email}", user.Email);
                throw;
            }

            return Unit.Value;
        }
    }

    public class ResetPasswordHandler(IUserService userService, ILogger<ResetPasswordHandler> logger)
        : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<ResetPasswordHandler> _logger = logger;
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByResetTokenAsync(request.Token);
            if (user == null)
            {
                _logger.LogWarning("Reset password request for unknown token: {Token}", request.Token);
                throw new Exception("Invalid reset token.");
            }

            // Hash the new password and update it
            var hashedPassword = PasswordService.HashPassword(request.NewPassword);
            await _userService.PasswordReset(user.Id, hashedPassword);
            _logger.LogInformation("Password reset successful for user ID: {UserId}", user.Id);
            return Unit.Value;
        }
    }
}