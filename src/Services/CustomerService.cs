using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
    }

    public class CustomerService(IRepository<Customer> repository) : ICustomerService
    {
        private readonly IRepository<Customer> _repository = repository;

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomersMessage, ex);
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerByIdMessage, ex);
            }
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            try
            {
                return await _repository.AddAsync(customer);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCustomerMessage, ex);
            }
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                return await _repository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateCustomerMessage, ex);
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null) return false;

                customer.IsActive = false; // Soft delete
                await _repository.UpdateAsync(customer); 
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteCustomerMessage, ex);
            }
        }
    }
}