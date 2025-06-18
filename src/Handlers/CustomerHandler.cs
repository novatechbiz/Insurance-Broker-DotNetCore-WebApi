using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InsuraNova.Data;
using InsuraNova.Models;
using InsuraNova.Services;
using MediatR;

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
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
    {
        private readonly ICustomerService _customerService;

        public GetAllCustomersHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomersAsync();
        }
    }

    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerByIdHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomerByIdAsync(request.Id);
        }
    }

    public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, Customer>
    {
        private readonly ICustomerService _customerService;

        public AddCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.AddCustomerAsync(request.Customer);
        }
    }

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.UpdateCustomerAsync(request.Customer);
        }
    }

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteCustomerHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.Id);
            if (customer == null || !customer.IsActive)
                return false;

            customer.IsActive = false;
            

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
