using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICustomerIdentificationTypeService
    {
        Task<IEnumerable<CustomerIdentificationType>> GetAllCustomerIdentificationTypesAsync();
        Task<CustomerIdentificationType> GetCustomerIdentificationTypeByIdAsync(int id);
        Task<CustomerIdentificationType> AddCustomerIdentificationTypeAsync(CustomerIdentificationType customerIdentificationType);
        Task<CustomerIdentificationType> UpdateCustomerIdentificationTypeAsync(CustomerIdentificationType customerIdentificationType);
        Task<bool> DeleteCustomerIdentificationTypeAsync(int id);
    }
    public class CustomerIdentificationTypeService(IRepository<CustomerIdentificationType> repository) : ICustomerIdentificationTypeService
    {
        private readonly IRepository<CustomerIdentificationType> _repository = repository;

        public async Task<IEnumerable<CustomerIdentificationType>> GetAllCustomerIdentificationTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerIdentificationTypesMessage, ex);
            }
        }

        public async Task<CustomerIdentificationType> GetCustomerIdentificationTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerIdentificationTypeByIdMessage, ex);
            }
        }

        public async Task<CustomerIdentificationType> AddCustomerIdentificationTypeAsync(CustomerIdentificationType customerIdentificationType)
        {
            try
            {
                return await _repository.AddAsync(customerIdentificationType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCompanyMessage, ex);
            }
        }

        public async Task<CustomerIdentificationType> UpdateCustomerIdentificationTypeAsync(CustomerIdentificationType customerIdentificationType)
        {
            try
            {
                return await _repository.UpdateAsync(customerIdentificationType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateCustomerIdentificationTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteCustomerIdentificationTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteCustomerIdentificationTypeMessage, ex);
            }
        }
    }
}
