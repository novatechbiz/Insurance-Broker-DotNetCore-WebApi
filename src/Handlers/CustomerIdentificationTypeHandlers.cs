using InsuraNova.Services;


namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllCustomerIdentificationTypesQuery : IRequest<IEnumerable<CustomerIdentificationType>>;
    public record GetCustomerIdentificationTypeByIdQuery(int Id) : IRequest<CustomerIdentificationType>;

    // Commands
    public record AddCustomerIdentificationTypeCommand(CustomerIdentificationType CustomerIdentificationType) : IRequest<CustomerIdentificationType>;
    public record UpdateCustomerIdentificationTypeCommand(CustomerIdentificationType CustomerIdentificationType) : IRequest<CustomerIdentificationType>;
    public record DeleteCustomerIdentificationTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllCustomerIdentificationTypesHandler(ICustomerIdentificationTypeService customerIdentificationTypeService) : IRequestHandler<GetAllCustomerIdentificationTypesQuery, IEnumerable<CustomerIdentificationType>>
    {
        private readonly ICustomerIdentificationTypeService _customerIdentificationTypeService = customerIdentificationTypeService;

        public async Task<IEnumerable<CustomerIdentificationType>> Handle(GetAllCustomerIdentificationTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerIdentificationTypeService.GetAllCustomerIdentificationTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetCustomerIdentificationTypeByIdHandler(ICustomerIdentificationTypeService customerIdentificationTypeService) : IRequestHandler<GetCustomerIdentificationTypeByIdQuery, CustomerIdentificationType>
    {
        private readonly ICustomerIdentificationTypeService _customerIdentificationTypeService = customerIdentificationTypeService;

        public async Task<CustomerIdentificationType> Handle(GetCustomerIdentificationTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerIdentificationTypeService.GetCustomerIdentificationTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddCustomerIdentificationTypeHandler(ICustomerIdentificationTypeService customerIdentificationTypeService) : IRequestHandler<AddCustomerIdentificationTypeCommand, CustomerIdentificationType>
    {
        private readonly ICustomerIdentificationTypeService _customerIdentificationTypeService = customerIdentificationTypeService;

        public async Task<CustomerIdentificationType> Handle(AddCustomerIdentificationTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerIdentificationTypeService.AddCustomerIdentificationTypeAsync(request.CustomerIdentificationType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateCustomerIdentificationTypeHandler(ICustomerIdentificationTypeService customerIdentificationTypeService) : IRequestHandler<UpdateCustomerIdentificationTypeCommand, CustomerIdentificationType>
    {
        private readonly ICustomerIdentificationTypeService _customerIdentificationTypeService = customerIdentificationTypeService;

        public async Task<CustomerIdentificationType> Handle(UpdateCustomerIdentificationTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerIdentificationTypeService.UpdateCustomerIdentificationTypeAsync(request.CustomerIdentificationType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteCustomerIdentificationTypeHandler(ICustomerIdentificationTypeService customerIdentificationTypeService) : IRequestHandler<DeleteCustomerIdentificationTypeCommand, bool>
    {
        private readonly ICustomerIdentificationTypeService _customerIdentificationTypeService = customerIdentificationTypeService;

        public async Task<bool> Handle(DeleteCustomerIdentificationTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _customerIdentificationTypeService.DeleteCustomerIdentificationTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
