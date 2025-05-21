using InsuraNova.Services;

namespace InsuraNova.Handlers
{
  
    public record GetAllUserRolesQuery : IRequest<IEnumerable<UserRole>>;

    public class GetAllUserRoleHandler(IUserRoleService userRoleService) : IRequestHandler<GetAllUserRolesQuery, IEnumerable<UserRole>>
    {
        private readonly IUserRoleService _userRoleService = userRoleService;

        public async Task<IEnumerable<UserRole>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userRoleService.GetAllUserRolesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
