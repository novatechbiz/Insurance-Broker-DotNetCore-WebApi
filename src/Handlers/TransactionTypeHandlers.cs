using InsuraNova.Models;
using InsuraNova.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InsuraNova.Handlers
{
    // Queries
    public record GetAllTransactionTypesQuery : IRequest<IEnumerable<TransactionType>>;
    public record GetTransactionTypeByIdQuery(int Id) : IRequest<TransactionType>;

    // Commands
    public record AddTransactionTypeCommand(TransactionType TransactionType) : IRequest<TransactionType>;
    public record UpdateTransactionTypeCommand(TransactionType TransactionType) : IRequest<TransactionType>;
    public record DeleteTransactionTypeCommand(int Id) : IRequest<bool>;

    // Handlers

    public class GetAllTransactionTypesHandler : IRequestHandler<GetAllTransactionTypesQuery, IEnumerable<TransactionType>>
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public GetAllTransactionTypesHandler(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<IEnumerable<TransactionType>> Handle(GetAllTransactionTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _transactionTypeService.GetTransactionTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetTransactionTypeByIdHandler : IRequestHandler<GetTransactionTypeByIdQuery, TransactionType>
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public GetTransactionTypeByIdHandler(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<TransactionType> Handle(GetTransactionTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _transactionTypeService.GetTransactionTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddTransactionTypeHandler : IRequestHandler<AddTransactionTypeCommand, TransactionType>
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public AddTransactionTypeHandler(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<TransactionType> Handle(AddTransactionTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _transactionTypeService.AddTransactionTypeAsync(request.TransactionType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateTransactionTypeHandler : IRequestHandler<UpdateTransactionTypeCommand, TransactionType>
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public UpdateTransactionTypeHandler(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<TransactionType> Handle(UpdateTransactionTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _transactionTypeService.UpdateTransactionTypeAsync(request.TransactionType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteTransactionTypeHandler : IRequestHandler<DeleteTransactionTypeCommand, bool>
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public DeleteTransactionTypeHandler(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<bool> Handle(DeleteTransactionTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _transactionTypeService.DeleteTransactionTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
