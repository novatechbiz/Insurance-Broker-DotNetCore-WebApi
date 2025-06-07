using InsuraNova.Services;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllSystemFunctionsQuery : IRequest<IEnumerable<SystemFunction>>;
    public record GetSystemFunctionByIdQuery(int Id) : IRequest<SystemFunction>;

    // Commands
    public record AddSystemFunctionCommand(SystemFunction SystemFunction) : IRequest<SystemFunction>;
    public record UpdateSystemFunctionCommand(SystemFunction SystemFunction) : IRequest<SystemFunction>;
    public record DeleteSystemFunctionCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllSystemFunctionsHandler(ISystemFunctionService systemFunctionService)
        : IRequestHandler<GetAllSystemFunctionsQuery, IEnumerable<SystemFunction>>
    {
        private readonly ISystemFunctionService _systemFunctionService = systemFunctionService;

        public async Task<IEnumerable<SystemFunction>> Handle(GetAllSystemFunctionsQuery request, CancellationToken cancellationToken)
        {
            return await _systemFunctionService.GetSystemFunctionsAsync();
        }
    }

    public class GetSystemFunctionByIdHandler(ISystemFunctionService systemFunctionService)
        : IRequestHandler<GetSystemFunctionByIdQuery, SystemFunction>
    {
        private readonly ISystemFunctionService _systemFunctionService = systemFunctionService;

        public async Task<SystemFunction> Handle(GetSystemFunctionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _systemFunctionService.GetSystemFunctionByIdAsync(request.Id);
        }
    }

    public class AddSystemFunctionHandler(ISystemFunctionService systemFunctionService)
        : IRequestHandler<AddSystemFunctionCommand, SystemFunction>
    {
        private readonly ISystemFunctionService _systemFunctionService = systemFunctionService;

        public async Task<SystemFunction> Handle(AddSystemFunctionCommand request, CancellationToken cancellationToken)
        {
            return await _systemFunctionService.AddSystemFunctionAsync(request.SystemFunction);
        }
    }

    public class UpdateSystemFunctionHandler(ISystemFunctionService systemFunctionService)
        : IRequestHandler<UpdateSystemFunctionCommand, SystemFunction>
    {
        private readonly ISystemFunctionService _systemFunctionService = systemFunctionService;

        public async Task<SystemFunction> Handle(UpdateSystemFunctionCommand request, CancellationToken cancellationToken)
        {
            return await _systemFunctionService.UpdateSystemFunctionAsync(request.SystemFunction);
        }
    }

    public class DeleteSystemFunctionHandler(ISystemFunctionService systemFunctionService)
        : IRequestHandler<DeleteSystemFunctionCommand, bool>
    {
        private readonly ISystemFunctionService _systemFunctionService = systemFunctionService;

        public async Task<bool> Handle(DeleteSystemFunctionCommand request, CancellationToken cancellationToken)
        {
            return await _systemFunctionService.DeleteSystemFunctionAsync(request.Id);
        }
    }
}
