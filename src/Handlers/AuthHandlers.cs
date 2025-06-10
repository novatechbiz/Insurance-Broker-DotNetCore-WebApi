using AutoMapper;
using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Services;
using InsuraNova.Validation;

namespace InsuraNova.Handlers
{
    public class AuthHandlers : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthHandlers> _logger;
        private readonly LoginRequestValidation _validator;

        public AuthHandlers(IUserRepository userRepository, IUserService userService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<AuthHandlers> logger, LoginRequestValidation validator)
        {
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _validator = validator;
        }

        public async Task<LoginResponse> Handle(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Login attempt with email: {Username}", loginRequest.Username);

                // Validate the login request
                var validationResult = _validator.Validate(loginRequest);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for email: {Username}. Errors: {Errors}", loginRequest.Username, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                    };
                }

                // Check if the user exists by email
                var user = await _userRepository.GetUserByUserNameAsync(loginRequest.Username.Trim());
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Username}", loginRequest.Username.Trim());
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email does not exist."
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

                // Store refresh token in the database or override the existing refresh token
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
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
}
