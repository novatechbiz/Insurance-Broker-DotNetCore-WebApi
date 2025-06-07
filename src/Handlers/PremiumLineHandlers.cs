using InsuraNova.Services;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllPremiumLinesQuery : IRequest<IEnumerable<PremiumLine>>;
    public record GetPremiumLineByIdQuery(int Id) : IRequest<PremiumLine>;

    // Commands
    public record AddPremiumLineCommand(PremiumLine PremiumLine) : IRequest<PremiumLine>;
    public record UpdatePremiumLineCommand(PremiumLine PremiumLine) : IRequest<PremiumLine>;
    public record DeletePremiumLineCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllPremiumLinesHandler(IPremiumLineService premiumLineService)
        : IRequestHandler<GetAllPremiumLinesQuery, IEnumerable<PremiumLine>>
    {
        private readonly IPremiumLineService _premiumLineService = premiumLineService;

        public async Task<IEnumerable<PremiumLine>> Handle(GetAllPremiumLinesQuery request, CancellationToken cancellationToken)
        {
            return await _premiumLineService.GetPremiumLinesAsync();
        }
    }

    public class GetPremiumLineByIdHandler(IPremiumLineService premiumLineService)
        : IRequestHandler<GetPremiumLineByIdQuery, PremiumLine>
    {
        private readonly IPremiumLineService _premiumLineService = premiumLineService;

        public async Task<PremiumLine> Handle(GetPremiumLineByIdQuery request, CancellationToken cancellationToken)
        {
            return await _premiumLineService.GetPremiumLineByIdAsync(request.Id);
        }
    }

    public class AddPremiumLineHandler(IPremiumLineService premiumLineService)
        : IRequestHandler<AddPremiumLineCommand, PremiumLine>
    {
        private readonly IPremiumLineService _premiumLineService = premiumLineService;

        public async Task<PremiumLine> Handle(AddPremiumLineCommand request, CancellationToken cancellationToken)
        {
            return await _premiumLineService.AddPremiumLineAsync(request.PremiumLine);
        }
    }

    public class UpdatePremiumLineHandler(IPremiumLineService premiumLineService)
        : IRequestHandler<UpdatePremiumLineCommand, PremiumLine>
    {
        private readonly IPremiumLineService _premiumLineService = premiumLineService;

        public async Task<PremiumLine> Handle(UpdatePremiumLineCommand request, CancellationToken cancellationToken)
        {
            return await _premiumLineService.UpdatePremiumLineAsync(request.PremiumLine);
        }
    }

    public class DeletePremiumLineHandler(IPremiumLineService premiumLineService)
        : IRequestHandler<DeletePremiumLineCommand, bool>
    {
        private readonly IPremiumLineService _premiumLineService = premiumLineService;

        public async Task<bool> Handle(DeletePremiumLineCommand request, CancellationToken cancellationToken)
        {
            return await _premiumLineService.DeletePremiumLineAsync(request.Id);
        }
    }
}
