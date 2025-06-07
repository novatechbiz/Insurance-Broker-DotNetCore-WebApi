using InsuraNova.Services;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllInsuranceCompaniesQuery : IRequest<IEnumerable<InsuranceCompany>>;
    public record GetInsuranceCompanyByIdQuery(int Id) : IRequest<InsuranceCompany>;

    // Commands
    public record AddInsuranceCompanyCommand(InsuranceCompany InsuranceCompany) : IRequest<InsuranceCompany>;
    public record UpdateInsuranceCompanyCommand(InsuranceCompany InsuranceCompany) : IRequest<InsuranceCompany>;
    public record DeleteInsuranceCompanyCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllInsuranceCompaniesHandler(IInsuranceCompanyService insuranceCompanyService)
        : IRequestHandler<GetAllInsuranceCompaniesQuery, IEnumerable<InsuranceCompany>>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService = insuranceCompanyService;

        public async Task<IEnumerable<InsuranceCompany>> Handle(GetAllInsuranceCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceCompanyService.GetInsuranceCompaniesAsync();
        }
    }

    public class GetInsuranceCompanyByIdHandler(IInsuranceCompanyService insuranceCompanyService)
        : IRequestHandler<GetInsuranceCompanyByIdQuery, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService = insuranceCompanyService;

        public async Task<InsuranceCompany> Handle(GetInsuranceCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceCompanyService.GetInsuranceCompanyByIdAsync(request.Id);
        }
    }

    public class AddInsuranceCompanyHandler(IInsuranceCompanyService insuranceCompanyService)
        : IRequestHandler<AddInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService = insuranceCompanyService;

        public async Task<InsuranceCompany> Handle(AddInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceCompanyService.AddInsuranceCompanyAsync(request.InsuranceCompany);
        }
    }

    public class UpdateInsuranceCompanyHandler(IInsuranceCompanyService insuranceCompanyService)
        : IRequestHandler<UpdateInsuranceCompanyCommand, InsuranceCompany>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService = insuranceCompanyService;

        public async Task<InsuranceCompany> Handle(UpdateInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceCompanyService.UpdateInsuranceCompanyAsync(request.InsuranceCompany);
        }
    }

    public class DeleteInsuranceCompanyHandler(IInsuranceCompanyService insuranceCompanyService)
        : IRequestHandler<DeleteInsuranceCompanyCommand, bool>
    {
        private readonly IInsuranceCompanyService _insuranceCompanyService = insuranceCompanyService;

        public async Task<bool> Handle(DeleteInsuranceCompanyCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceCompanyService.DeleteInsuranceCompanyAsync(request.Id);
        }
    }
}
