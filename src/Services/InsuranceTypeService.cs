using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;
using InsuraNova.Models;

namespace InsuraNova.Services
{
    public interface IInsuranceTypeService
    {
        Task<IEnumerable<InsuranceType>> GetInsuranceTypesAsync();
        Task<InsuranceType> GetInsuranceTypeByIdAsync(int id);
        Task<InsuranceType> AddInsuranceTypeAsync(InsuranceType insuranceType);
        Task<InsuranceType> UpdateInsuranceTypeAsync(InsuranceType insuranceType);
        Task<bool> DeleteInsuranceTypeAsync(int id);
    }

    public class InsuranceTypeService(IRepository<InsuranceType> repository) : IInsuranceTypeService
    {
        private readonly IRepository<InsuranceType> _repository = repository;

        public async Task<IEnumerable<InsuranceType>> GetInsuranceTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveInsuranceTypesMessage, ex);
            }
        }

        public async Task<InsuranceType> GetInsuranceTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveInsuranceTypeByIdMessage, ex);
            }
        }

        public async Task<InsuranceType> AddInsuranceTypeAsync(InsuranceType insuranceType)
        {
            try
            {
                return await _repository.AddAsync(insuranceType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddInsuranceTypeMessage, ex);
            }
        }

        public async Task<InsuranceType> UpdateInsuranceTypeAsync(InsuranceType insuranceType)
        {
            try
            {
                return await _repository.UpdateAsync(insuranceType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateInsuranceTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteInsuranceTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteInsuranceTypeMessage, ex);
            }
        }
    }
}
