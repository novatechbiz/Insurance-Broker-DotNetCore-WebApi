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
    public record GetAllCurrenciesQuery : IRequest<IEnumerable<Currency>>;
    public record GetCurrencyByIdQuery(int Id) : IRequest<Currency>;

    // Commands
    public record AddCurrencyCommand(Currency Currency) : IRequest<Currency>;
    public record UpdateCurrencyCommand(Currency Currency) : IRequest<Currency>;
    public record DeleteCurrencyCommand(int Id) : IRequest<bool>;

    // Handlers
    public class GetAllCurrenciesHandler : IRequestHandler<GetAllCurrenciesQuery, IEnumerable<Currency>>
    {
        private readonly ICurrencyService _currencyService;

        public GetAllCurrenciesHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IEnumerable<Currency>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _currencyService.GetCurrenciesAsync();
        }
    }

    public class GetCurrencyByIdHandler : IRequestHandler<GetCurrencyByIdQuery, Currency>
    {
        private readonly ICurrencyService _currencyService;

        public GetCurrencyByIdHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<Currency> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _currencyService.GetCurrencyByIdAsync(request.Id);
        }
    }

    public class AddCurrencyHandler : IRequestHandler<AddCurrencyCommand, Currency>
    {
        private readonly ICurrencyService _currencyService;

        public AddCurrencyHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<Currency> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
        {
            return await _currencyService.AddCurrencyAsync(request.Currency);
        }
    }

    public class UpdateCurrencyHandler : IRequestHandler<UpdateCurrencyCommand, Currency>
    {
        private readonly ICurrencyService _currencyService;

        public UpdateCurrencyHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<Currency> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            return await _currencyService.UpdateCurrencyAsync(request.Currency);
        }
    }

    public class DeleteCurrencyHandler : IRequestHandler<DeleteCurrencyCommand, bool>
    {
        private readonly ICurrencyService _currencyService;

        public DeleteCurrencyHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            return await _currencyService.DeleteCurrencyAsync(request.Id);
        }
    }
}
