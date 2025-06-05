using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllGenderTypesQuery : IRequest<IEnumerable<GenderType>>;
    public record GetGenderTypeByIdQuery(int Id) : IRequest<GenderType>;

    // Commands
    public record AddGenderTypeCommand(GenderType GenderType) : IRequest<GenderType>;
    public record UpdateGenderTypeCommand(GenderType GenderType) : IRequest<GenderType>;
    public record DeleteGenderTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllGenderTypesHandler(IGenderTypeService genderTypeService) : IRequestHandler<GetAllGenderTypesQuery, IEnumerable<GenderType>>
    {
        private readonly IGenderTypeService _genderTypeService = genderTypeService;

        public async Task<IEnumerable<GenderType>> Handle(GetAllGenderTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _genderTypeService.GetAllGenderTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetGenderTypeByIdHandler(IGenderTypeService genderTypeService) : IRequestHandler<GetGenderTypeByIdQuery, GenderType>
    {
        private readonly IGenderTypeService _genderTypeService = genderTypeService;

        public async Task<GenderType> Handle(GetGenderTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _genderTypeService.GetGenderTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddGenderTypeHandler(IGenderTypeService genderTypeService) : IRequestHandler<AddGenderTypeCommand, GenderType>
    {
        private readonly IGenderTypeService _genderTypeService = genderTypeService;

        public async Task<GenderType> Handle(AddGenderTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _genderTypeService.AddGenderTypeAsync(request.GenderType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateGenderTypeHandler(IGenderTypeService genderTypeService) : IRequestHandler<UpdateGenderTypeCommand, GenderType>
    {
        private readonly IGenderTypeService _genderTypeService = genderTypeService;

        public async Task<GenderType> Handle(UpdateGenderTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _genderTypeService.UpdateGenderTypeAsync(request.GenderType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteGenderTypeHandler(IGenderTypeService genderTypeService) : IRequestHandler<DeleteGenderTypeCommand, bool>
    {
        private readonly IGenderTypeService _genderTypeService = genderTypeService;

        public async Task<bool> Handle(DeleteGenderTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _genderTypeService.DeleteGenderTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
