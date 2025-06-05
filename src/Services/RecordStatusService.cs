using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IRecordStatusService
    {
        Task<IEnumerable<RecordStatus>> GetRecordStatusesAsync();
        Task<RecordStatus> GetRecordStatusByIdAsync(int id);
        Task<RecordStatus> AddRecordStatusAsync(RecordStatus recordStatus);
        Task<RecordStatus> UpdateRecordStatusAsync(RecordStatus recordStatus);
        Task<bool> DeleteRecordStatusAsync(int id);
    }
    public class RecordStatusService(IRepository<RecordStatus> repository) : IRecordStatusService
    {
        private readonly IRepository<RecordStatus> _repository = repository;

        public async Task<IEnumerable<RecordStatus>> GetRecordStatusesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveRecordStatusesMessage, ex);
            }
        }

        public async Task<RecordStatus> GetRecordStatusByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveRecordStatusByIdMessage, ex);
            }
        }

        public async Task<RecordStatus> AddRecordStatusAsync(RecordStatus recordStatus)
        {
            try
            {
                return await _repository.AddAsync(recordStatus);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddRecordStatusMessage, ex);
            }
        }

        public async Task<RecordStatus> UpdateRecordStatusAsync(RecordStatus recordStatus)
        {
            try
            {
                return await _repository.UpdateAsync(recordStatus);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateRecordStatusMessage, ex);
            }
        }

        public async Task<bool> DeleteRecordStatusAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteRecordStatusMessage, ex);
            }
        }
    }
}
