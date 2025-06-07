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
    public record GetAllEntryTypesQuery : IRequest<IEnumerable<EntryType>>;
    public record GetEntryTypeByIdQuery(int Id) : IRequest<EntryType>;

    // Commands
    public record AddEntryTypeCommand(EntryType EntryType) : IRequest<EntryType>;
    public record UpdateEntryTypeCommand(EntryType EntryType) : IRequest<EntryType>;
    public record DeleteEntryTypeCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllEntryTypesHandler : IRequestHandler<GetAllEntryTypesQuery, IEnumerable<EntryType>>
    {
        private readonly IEntryTypeService _entryTypeService;

        public GetAllEntryTypesHandler(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        public async Task<IEnumerable<EntryType>> Handle(GetAllEntryTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _entryTypeService.GetEntryTypesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetEntryTypeByIdHandler : IRequestHandler<GetEntryTypeByIdQuery, EntryType>
    {
        private readonly IEntryTypeService _entryTypeService;

        public GetEntryTypeByIdHandler(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        public async Task<EntryType> Handle(GetEntryTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _entryTypeService.GetEntryTypeByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddEntryTypeHandler : IRequestHandler<AddEntryTypeCommand, EntryType>
    {
        private readonly IEntryTypeService _entryTypeService;

        public AddEntryTypeHandler(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        public async Task<EntryType> Handle(AddEntryTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _entryTypeService.AddEntryTypeAsync(request.EntryType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateEntryTypeHandler : IRequestHandler<UpdateEntryTypeCommand, EntryType>
    {
        private readonly IEntryTypeService _entryTypeService;

        public UpdateEntryTypeHandler(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        public async Task<EntryType> Handle(UpdateEntryTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _entryTypeService.UpdateEntryTypeAsync(request.EntryType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteEntryTypeHandler : IRequestHandler<DeleteEntryTypeCommand, bool>
    {
        private readonly IEntryTypeService _entryTypeService;

        public DeleteEntryTypeHandler(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        public async Task<bool> Handle(DeleteEntryTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _entryTypeService.DeleteEntryTypeAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
