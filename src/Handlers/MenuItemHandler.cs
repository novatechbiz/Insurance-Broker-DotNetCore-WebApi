using InsuraNova.Services;
using MediatR;

namespace InsuraNova.Handlers
{
    public record GetMenuItemsQuery : IRequest<List<MenuItem>>;

    public class GetMenuItemsHandler : IRequestHandler<GetMenuItemsQuery, List<MenuItem>>
    {
        private readonly IMenuItemService _service;

        public GetMenuItemsHandler(IMenuItemService service)
        {
            _service = service;
        }

        public Task<List<MenuItem>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetMenuItems();
            return Task.FromResult(items);
        }
    }
}
