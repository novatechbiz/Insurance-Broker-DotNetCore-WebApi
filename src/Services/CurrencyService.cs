using InsuraNova.Helpers;
using InsuraNova.Repositories;

using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency> GetCurrencyByIdAsync(int id);
        Task<Currency> AddCurrencyAsync(Currency currency);
        Task<Currency> UpdateCurrencyAsync(Currency currency);
        Task<bool> DeleteCurrencyAsync(int id);
    }
    public class CurrencyService(IRepository<Currency> repository) : ICurrencyService
    {
        private readonly IRepository<Currency> _repository = repository;

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCurrenciesMessage, ex);
            }
        }

        public async Task<Currency> GetCurrencyByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCurrencyByIdMessage, ex);
            }
        }

        public async Task<Currency> AddCurrencyAsync(Currency currency)
        {
            try
            {
                return await _repository.AddAsync(currency);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCurrencyMessage, ex);
            }
        }

        public async Task<Currency> UpdateCurrencyAsync(Currency currency)
        {
            try
            {
                return await _repository.UpdateAsync(currency);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateCurrencyMessage, ex);
            }
        }

        public async Task<bool> DeleteCurrencyAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteCurrencyMessage, ex);
            }
        }
    }
}
