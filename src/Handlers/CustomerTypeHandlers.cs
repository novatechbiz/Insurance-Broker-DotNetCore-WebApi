using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllCustomerTypesQuery : IRequest<IEnumerable<CustomerType>>;
    public record GetCustomerTypeByIdQuery(int Id) : IRequest<CustomerType>;

    // Commands
    public record AddCustomerTypeCommand(CustomerType CustomerType) : IRequest<CustomerType>;
    public record UpdateCustomerTypeCommand(CustomerType CustomerType) : IRequest<CustomerType>;
    public record DeleteCustomerTypeCommand(int Id) : IRequest<bool>;


    // Handlers
    public class GetAllCustomerTypesHandler(ICustomerTypeService customerTypeService) : IRequestHandler<GetAllCustomerTypesQuery, IEnumerable<CustomerType>>
    {
        private readonly ICustomerTypeService _customerTypeService = customerTypeService;

        public async Task<IEnumerable<CustomerType>> Handle(GetAllCustomerTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerTypeService.GetAllCustomerTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetCustomerTypeByIdHandler(ICustomerTypeService customerTypeService) : IRequestHandler<GetCustomerTypeByIdQuery, CustomerType>
    {
        private readonly ICustomerTypeService _customerTypeService = customerTypeService;

        public async Task<CustomerType> Handle(GetCustomerTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerTypeService.GetCustomerTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddCustomerTypeHandler(ICustomerTypeService customerTypeService) : IRequestHandler<AddCustomerTypeCommand, CustomerType>
    {
        private readonly ICustomerTypeService _customerTypeService = customerTypeService;

        public async Task<CustomerType> Handle(AddCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerTypeService.AddCustomerTypeAsync(request.CustomerType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateCustomerTypeHandler(ICustomerTypeService customerTypeService) : IRequestHandler<UpdateCustomerTypeCommand, CustomerType>
    {
        private readonly ICustomerTypeService _customerTypeService = customerTypeService;

        public async Task<CustomerType> Handle(UpdateCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerTypeService.UpdateCustomerTypeAsync(request.CustomerType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteCustomerTypeHandler(ICustomerTypeService customerTypeService) : IRequestHandler<DeleteCustomerTypeCommand, bool>
    {
        private readonly ICustomerTypeService _customerTypeService = customerTypeService;

        public async Task<bool> Handle(DeleteCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerTypeService.DeleteCustomerTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
