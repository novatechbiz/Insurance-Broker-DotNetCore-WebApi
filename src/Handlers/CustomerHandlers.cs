using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllCustomersQuery : IRequest<IEnumerable<Customer>>;
    public record GetCustomerByIdQuery(int Id) : IRequest<Customer>;

    // Commands
    public record AddCustomerCommand(Customer Customer) : IRequest<Customer>;
    public record UpdateCustomerCommand(Customer Customer) : IRequest<Customer>;
    public record DeleteCustomerCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllCustomersHandler(ICustomerService customerService) : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerService.GetAllCustomersAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetCustomerByIdHandler(ICustomerService customerService) : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerService.GetCustomerByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddCustomerHandler(ICustomerService customerService) : IRequestHandler<AddCustomerCommand, Customer>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerService.AddCustomerAsync(request.Customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateCustomerHandler(ICustomerService customerService) : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerService.UpdateCustomerAsync(request.Customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteCustomerHandler(ICustomerService customerService) : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerService.DeleteCustomerAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}