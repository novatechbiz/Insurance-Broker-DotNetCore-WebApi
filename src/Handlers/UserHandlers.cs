using AutoMapper;
using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;
    public record GetUserByIdQuery(int Id) : IRequest<UserProfile>;

    // Commands
    public record AddUserCommand(UserProfile User) : IRequest<UserProfile>;
    public record UpdateUserCommand(UserProfile User) : IRequest<UserProfile>;
    public record DeleteUserCommand(int Id) : IRequest<bool>;
    public record ForgotPasswordCommand(string Email) : IRequest<bool>;


    // Handlers
    public class GetAllUsersHandler(IUserService userService, IMapper mapper) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetAllUsersAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(user);
                return userDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetUserByIdHandler(IUserService userService) : IRequestHandler<GetUserByIdQuery, UserProfile>
    {
        private readonly IUserService _userService = userService;

        public async Task<UserProfile> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.GetUserByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddUserHandler(IUserService userService) : IRequestHandler<AddUserCommand, UserProfile>
    {
        private readonly IUserService _userService = userService;

        public async Task<UserProfile> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.AddUserAsync(request.User);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUserCommand, UserProfile>
    {
        private readonly IUserService _userService = userService;

        public async Task<UserProfile> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.UpdateUserAsync(request.User);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteUserHandler(IUserService userService) : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserService _userService = userService;

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.DeleteUserAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
   
}
