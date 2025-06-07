using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICustomerTypeService
    {
        Task<IEnumerable<CustomerType>> GetAllCustomerTypesAsync();
        Task<CustomerType> GetCustomerTypeByIdAsync(int id);
        Task<CustomerType> AddCustomerTypeAsync(CustomerType customerType);
        Task<CustomerType> UpdateCustomerTypeAsync(CustomerType customerType);
        Task<bool> DeleteCustomerTypeAsync(int id);
    }
    public class CustomerTypeService(IRepository<CustomerType> repository) : ICustomerTypeService
    {
        private readonly IRepository<CustomerType> _repository = repository;

        public async Task<IEnumerable<CustomerType>> GetAllCustomerTypesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerTypesMessage, ex);
            }
        }

        public async Task<CustomerType> GetCustomerTypeByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerTypeByIdMessage, ex);
            }
        }

        public async Task<CustomerType> AddCustomerTypeAsync(CustomerType customerType)
        {
            try
            {
                return await _repository.AddAsync(customerType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCustomerTypeMessage, ex);
            }
        }

        public async Task<CustomerType> UpdateCustomerTypeAsync(CustomerType customerType)
        {
            try
            {
                return await _repository.UpdateAsync(customerType);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateCustomerTypeMessage, ex);
            }
        }

        public async Task<bool> DeleteCustomerTypeAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteCustomerTypeMessage, ex);
            }
        }
    }
}
