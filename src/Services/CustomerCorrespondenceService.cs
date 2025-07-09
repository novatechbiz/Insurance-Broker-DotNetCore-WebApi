using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{

    public interface ICustomerCorrespondenceService
    {
        Task<IEnumerable<CustomerCorrespondence>> GetCustomerCorrespondencesAsync();
        Task<CustomerCorrespondence> GetCustomerCorrespondenceByIdAsync(int id);
        Task<IEnumerable<CustomerCorrespondence>> GetCustomerCorrespondenceByCustomerIdAsync(int customerId);
        Task<CustomerCorrespondence> AddCustomerCorrespondenceAsync(CustomerCorrespondence customerCorrespondence);
        Task<CustomerCorrespondence> UpdateCustomerCorrespondenceAsync(CustomerCorrespondence customerCorrespondence);
        Task<bool> DeleteCustomerCorrespondenceAsync(int id);
    }

    public class CustomerCorrespondenceService(ICustomerCorrespondenceRepository repository) : ICustomerCorrespondenceService
    {
        private readonly ICustomerCorrespondenceRepository _repository = repository;

        public async Task<CustomerCorrespondence> AddCustomerCorrespondenceAsync(CustomerCorrespondence customerCorrespondence)
        {
            try
            {
                return await _repository.AddAsync(customerCorrespondence);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCustomerCorrespondenceMessage, ex);
            }
        }


        public async Task<CustomerCorrespondence> GetCustomerCorrespondenceByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerCorrespondenceByIdMessage, ex);
            }
        }

        public async Task<IEnumerable<CustomerCorrespondence>> GetCustomerCorrespondenceByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _repository.GetByCustomerIdAsync(customerId);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCustomerCorrespondenceByCustomerIdMessage, ex);
            }
        }


        public Task<IEnumerable<CustomerCorrespondence>> GetCustomerCorrespondencesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCustomerCorrespondenceAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<CustomerCorrespondence> UpdateCustomerCorrespondenceAsync(CustomerCorrespondence customerCorrespondence)
        {
            throw new NotImplementedException();
        }

    }
}
