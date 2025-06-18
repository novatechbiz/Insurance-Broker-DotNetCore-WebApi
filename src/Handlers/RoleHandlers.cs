using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllRolesQuery : IRequest<IEnumerable<Role>>;
    public record GetRoleByIdQuery(int Id) : IRequest<Role>;

    // Commands
    public record AddRoleCommand(Role Role) : IRequest<Role>;
    public record UpdateRoleCommand(Role Role) : IRequest<Role>;
    public record DeleteRoleCommand(int Id) : IRequest<bool>;


    // Handlers
    public class GetAllRolesHandler(IRoleService roleService) : IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _roleService.GetAllRolesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetRoleByIdHandler(IRoleService roleService) : IRequestHandler<GetRoleByIdQuery, Role>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _roleService.GetRoleByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddRoleHandler(IRoleService roleService) : IRequestHandler<AddRoleCommand, Role>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Role> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _roleService.AddRoleAsync(request.Role);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateRoleHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand, Role>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _roleService.UpdateRoleAsync(request.Role);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteRoleHandler(IRoleService roleService) : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _roleService.DeleteRoleAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
