using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InsuraNova.Models;
using InsuraNova.Services;
using MediatR;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllInsuranceTypesQuery : IRequest<IEnumerable<InsuranceType>>;
    public record GetInsuranceTypeByIdQuery(int Id) : IRequest<InsuranceType>;

    // Commands
    public record AddInsuranceTypeCommand(InsuranceType InsuranceType) : IRequest<InsuranceType>;
    public record UpdateInsuranceTypeCommand(InsuranceType InsuranceType) : IRequest<InsuranceType>;
    public record DeleteInsuranceTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllInsuranceTypesHandler : IRequestHandler<GetAllInsuranceTypesQuery, IEnumerable<InsuranceType>>
    {
        private readonly IInsuranceTypeService _insuranceTypeService;

        public GetAllInsuranceTypesHandler(IInsuranceTypeService insuranceTypeService)
        {
            _insuranceTypeService = insuranceTypeService;
        }

        public async Task<IEnumerable<InsuranceType>> Handle(GetAllInsuranceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _insuranceTypeService.GetInsuranceTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetInsuranceTypeByIdHandler : IRequestHandler<GetInsuranceTypeByIdQuery, InsuranceType>
    {
        private readonly IInsuranceTypeService _insuranceTypeService;

        public GetInsuranceTypeByIdHandler(IInsuranceTypeService insuranceTypeService)
        {
            _insuranceTypeService = insuranceTypeService;
        }

        public async Task<InsuranceType> Handle(GetInsuranceTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _insuranceTypeService.GetInsuranceTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddInsuranceTypeHandler : IRequestHandler<AddInsuranceTypeCommand, InsuranceType>
    {
        private readonly IInsuranceTypeService _insuranceTypeService;

        public AddInsuranceTypeHandler(IInsuranceTypeService insuranceTypeService)
        {
            _insuranceTypeService = insuranceTypeService;
        }

        public async Task<InsuranceType> Handle(AddInsuranceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _insuranceTypeService.AddInsuranceTypeAsync(request.InsuranceType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateInsuranceTypeHandler : IRequestHandler<UpdateInsuranceTypeCommand, InsuranceType>
    {
        private readonly IInsuranceTypeService _insuranceTypeService;

        public UpdateInsuranceTypeHandler(IInsuranceTypeService insuranceTypeService)
        {
            _insuranceTypeService = insuranceTypeService;
        }

        public async Task<InsuranceType> Handle(UpdateInsuranceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _insuranceTypeService.UpdateInsuranceTypeAsync(request.InsuranceType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteInsuranceTypeHandler : IRequestHandler<DeleteInsuranceTypeCommand, bool>
    {
        private readonly IInsuranceTypeService _insuranceTypeService;

        public DeleteInsuranceTypeHandler(IInsuranceTypeService insuranceTypeService)
        {
            _insuranceTypeService = insuranceTypeService;
        }

        public async Task<bool> Handle(DeleteInsuranceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _insuranceTypeService.DeleteInsuranceTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
