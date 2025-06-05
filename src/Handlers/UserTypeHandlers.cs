using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllUserTypesQuery : IRequest<IEnumerable<UserType>>;
    public record GetUserTypeByIdQuery(int Id) : IRequest<UserType>;

    // Commands
    public record AddUserTypeCommand(UserType UserType) : IRequest<UserType>;
    public record UpdateUserTypeCommand(UserType UserType) : IRequest<UserType>;
    public record DeleteUserTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllUserTypesHandler(IUserTypeService userTypeService) : IRequestHandler<GetAllUserTypesQuery, IEnumerable<UserType>>
    {
        private readonly IUserTypeService _userTypeService = userTypeService;

        public async Task<IEnumerable<UserType>> Handle(GetAllUserTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userTypeService.GetAllUserTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetUserTypeByIdHandler(IUserTypeService userTypeService) : IRequestHandler<GetUserTypeByIdQuery, UserType>
    {
        private readonly IUserTypeService _userTypeService = userTypeService;

        public async Task<UserType> Handle(GetUserTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userTypeService.GetUserTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddUserTypeHandler(IUserTypeService userTypeService) : IRequestHandler<AddUserTypeCommand, UserType>
    {
        private readonly IUserTypeService _userTypeService = userTypeService;

        public async Task<UserType> Handle(AddUserTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userTypeService.AddUserTypeAsync(request.UserType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateUserTypeHandler(IUserTypeService userTypeService) : IRequestHandler<UpdateUserTypeCommand, UserType>
    {
        private readonly IUserTypeService _userTypeService = userTypeService;

        public async Task<UserType> Handle(UpdateUserTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userTypeService.UpdateUserTypeAsync(request.UserType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteUserTypeHandler(IUserTypeService UserTypeService) : IRequestHandler<DeleteUserTypeCommand, bool>
    {
        private readonly IUserTypeService _userTypeService = UserTypeService;

        public async Task<bool> Handle(DeleteUserTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userTypeService.DeleteUserTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
