using InsuraNova.Services;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllCompanyTypesQuery : IRequest<IEnumerable<CompanyType>>;
    public record GetCompanyTypeByIdQuery(int Id) : IRequest<CompanyType>;

    // Commands
    public record AddCompanyTypeCommand(CompanyType CompanyType) : IRequest<CompanyType>;
    public record UpdateCompanyTypeCommand(CompanyType CompanyType) : IRequest<CompanyType>;
    public record DeleteCompanyTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllCompanyTypesHandler(ICompanyTypeService companyTypeService)
        : IRequestHandler<GetAllCompanyTypesQuery, IEnumerable<CompanyType>>
    {
        private readonly ICompanyTypeService _companyTypeService = companyTypeService;

        public async Task<IEnumerable<CompanyType>> Handle(GetAllCompanyTypesQuery request, CancellationToken cancellationToken)
        {
            return await _companyTypeService.GetCompanyTypesAsync();
        }
    }

    public class GetCompanyTypeByIdHandler(ICompanyTypeService companyTypeService)
        : IRequestHandler<GetCompanyTypeByIdQuery, CompanyType>
    {
        private readonly ICompanyTypeService _companyTypeService = companyTypeService;

        public async Task<CompanyType> Handle(GetCompanyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _companyTypeService.GetCompanyTypeByIdAsync(request.Id);
        }
    }

    public class AddCompanyTypeHandler(ICompanyTypeService companyTypeService)
        : IRequestHandler<AddCompanyTypeCommand, CompanyType>
    {
        private readonly ICompanyTypeService _companyTypeService = companyTypeService;

        public async Task<CompanyType> Handle(AddCompanyTypeCommand request, CancellationToken cancellationToken)
        {
            return await _companyTypeService.AddCompanyTypeAsync(request.CompanyType);
        }
    }

    public class UpdateCompanyTypeHandler(ICompanyTypeService companyTypeService)
        : IRequestHandler<UpdateCompanyTypeCommand, CompanyType>
    {
        private readonly ICompanyTypeService _companyTypeService = companyTypeService;

        public async Task<CompanyType> Handle(UpdateCompanyTypeCommand request, CancellationToken cancellationToken)
        {
            return await _companyTypeService.UpdateCompanyTypeAsync(request.CompanyType);
        }
    }

    public class DeleteCompanyTypeHandler(ICompanyTypeService companyTypeService)
        : IRequestHandler<DeleteCompanyTypeCommand, bool>
    {
        private readonly ICompanyTypeService _companyTypeService = companyTypeService;

        public async Task<bool> Handle(DeleteCompanyTypeCommand request, CancellationToken cancellationToken)
        {
            return await _companyTypeService.DeleteCompanyTypeAsync(request.Id);
        }
    }
}
