using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;


namespace InsuraNova.Services
{
    public interface IEntryTypeService
    {
        Task<IEnumerable<EntryType>> GetEntryTypesAsync();
        Task<EntryType> GetEntryTypeByIdAsync(int id);
        Task<EntryType> AddEntryTypeAsync(EntryType entryType);
        Task<EntryType> UpdateEntryTypeAsync(EntryType entryType);
        Task<bool> DeleteEntryTypeAsync(int id);
    }

    public class EntryTypeService : IEntryTypeService
    {
        private readonly IRepository<EntryType> _repository;

        public EntryTypeService(IRepository<EntryType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EntryType>> GetEntryTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveEntryTypesMessage, ex);
            }
        }

        public async Task<EntryType> GetEntryTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveEntryTypeByIdMessage, ex);
            }
        }

        public async Task<EntryType> AddEntryTypeAsync(EntryType entryType)
        {
            try
            {
                return await _repository.AddAsync(entryType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddEntryTypeMessage, ex);
            }
        }

        public async Task<EntryType> UpdateEntryTypeAsync(EntryType entryType)
        {
            try
            {
                return await _repository.UpdateAsync(entryType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateEntryTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteEntryTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteEntryTypeMessage, ex);
            }
        }
    }
}
