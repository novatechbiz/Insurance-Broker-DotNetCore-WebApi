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
    public record GetAllRecordStatusesQuery : IRequest<IEnumerable<RecordStatus>>;
    public record GetRecordStatusByIdQuery(int Id) : IRequest<RecordStatus>;

    // Commands
    public record AddRecordStatusCommand(RecordStatus RecordStatus) : IRequest<RecordStatus>;
    public record UpdateRecordStatusCommand(RecordStatus RecordStatus) : IRequest<RecordStatus>;
    public record DeleteRecordStatusCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllRecordStatusesHandler : IRequestHandler<GetAllRecordStatusesQuery, IEnumerable<RecordStatus>>
    {
        private readonly IRecordStatusService _recordStatusService;

        public GetAllRecordStatusesHandler(IRecordStatusService recordStatusService)
        {
            _recordStatusService = recordStatusService;
        }

        public async Task<IEnumerable<RecordStatus>> Handle(GetAllRecordStatusesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recordStatusService.GetRecordStatusesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class GetRecordStatusByIdHandler : IRequestHandler<GetRecordStatusByIdQuery, RecordStatus>
    {
        private readonly IRecordStatusService _recordStatusService;

        public GetRecordStatusByIdHandler(IRecordStatusService recordStatusService)
        {
            _recordStatusService = recordStatusService;
        }

        public async Task<RecordStatus> Handle(GetRecordStatusByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recordStatusService.GetRecordStatusByIdAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AddRecordStatusHandler : IRequestHandler<AddRecordStatusCommand, RecordStatus>
    {
        private readonly IRecordStatusService _recordStatusService;

        public AddRecordStatusHandler(IRecordStatusService recordStatusService)
        {
            _recordStatusService = recordStatusService;
        }

        public async Task<RecordStatus> Handle(AddRecordStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recordStatusService.AddRecordStatusAsync(request.RecordStatus);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class UpdateRecordStatusHandler : IRequestHandler<UpdateRecordStatusCommand, RecordStatus>
    {
        private readonly IRecordStatusService _recordStatusService;

        public UpdateRecordStatusHandler(IRecordStatusService recordStatusService)
        {
            _recordStatusService = recordStatusService;
        }

        public async Task<RecordStatus> Handle(UpdateRecordStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recordStatusService.UpdateRecordStatusAsync(request.RecordStatus);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class DeleteRecordStatusHandler : IRequestHandler<DeleteRecordStatusCommand, bool>
    {
        private readonly IRecordStatusService _recordStatusService;

        public DeleteRecordStatusHandler(IRecordStatusService recordStatusService)
        {
            _recordStatusService = recordStatusService;
        }

        public async Task<bool> Handle(DeleteRecordStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _recordStatusService.DeleteRecordStatusAsync(request.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
