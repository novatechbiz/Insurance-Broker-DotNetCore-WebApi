using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionType>> GetTransactionTypesAsync();
        Task<TransactionType> GetTransactionTypeByIdAsync(int id);
        Task<TransactionType> AddTransactionTypeAsync(TransactionType transactionType);
        Task<TransactionType> UpdateTransactionTypeAsync(TransactionType transactionType);
        Task<bool> DeleteTransactionTypeAsync(int id);
    }

    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly IRepository<TransactionType> _repository;

        public TransactionTypeService(IRepository<TransactionType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TransactionType>> GetTransactionTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveTransactionTypesMessage, ex);
            }
        }

        public async Task<TransactionType> GetTransactionTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveTransactionTypeByIdMessage, ex);
            }
        }

        public async Task<TransactionType> AddTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                return await _repository.AddAsync(transactionType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddTransactionTypeMessage, ex);
            }
        }

        public async Task<TransactionType> UpdateTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                return await _repository.UpdateAsync(transactionType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateTransactionTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteTransactionTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteTransactionTypeMessage, ex);
            }
        }
    }
}
