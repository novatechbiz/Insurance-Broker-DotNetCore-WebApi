using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IGenderTypeService
    {
        Task<IEnumerable<GenderType>> GetAllGenderTypesAsync();
        Task<GenderType> GetGenderTypeByIdAsync(int id);
        Task<GenderType> AddGenderTypeAsync(GenderType genderType);
        Task<GenderType> UpdateGenderTypeAsync(GenderType genderType);
        Task<bool> DeleteGenderTypeAsync(int id);
    }
    public class GenderTypeService(IRepository<GenderType> repository) : IGenderTypeService 
    {
        private readonly IRepository<GenderType> _repository = repository;

        public async Task<IEnumerable<GenderType>> GetAllGenderTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveGenderTypesMessage, ex);
            }
        }

        public async Task<GenderType> GetGenderTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveGenderTypeByIdMessage, ex);
            }
        }

        public async Task<GenderType> AddGenderTypeAsync(GenderType genderType)
        {
            try
            {
                return await _repository.AddAsync(genderType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddGenderTypeMessage, ex);
            }
        }

        public async Task<GenderType> UpdateGenderTypeAsync(GenderType genderType)
        {
            try
            {
                return await _repository.UpdateAsync(genderType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateGenderTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteGenderTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteGenderTypeMessage, ex);
            }
        }
    }
}
