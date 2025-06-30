using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(int id, Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly ICustomerHistoryLogService _historyLogService;

        public CustomerService(IRepository<Customer> repository, ICustomerHistoryLogService historyLogService)
        {
            _repository = repository;
            _historyLogService = historyLogService;
        }

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

        public async Task<Customer> UpdateCustomerAsync(int id, Customer customer)
        {
            try
            {
                var existingCustomer = await _repository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    throw new Exception("Customer not found.");
                }

                
                var oldCustomer = new CustomerHistoryLog
                {
                    CustomerId = existingCustomer.Id,
                    CompanyId = existingCustomer.CompanyId,
                    CustomerIdentificationTypeId = existingCustomer.CustomerIdentificationTypeId,
                    CustomerTypeId = existingCustomer.CustomerTypeId,
                    CustomerName = existingCustomer.CustomerName,
                    IdentificationNo = existingCustomer.IdentificationNo,
                    FullName = existingCustomer.FullName,
                    ContactNo = existingCustomer.ContactNo,
                    WhatsAppNo = existingCustomer.WhatsAppNo,
                    EmailAddress = existingCustomer.EmailAddress,
                    GenderTypeId = existingCustomer.GenderTypeId,
                    DateOfBirth = existingCustomer.DateOfBirth,
                    RecordStatusId = existingCustomer.RecordStatusId,
                    EnteredBy = existingCustomer.EnteredBy,
                    EnteredDate = existingCustomer.EnteredDate
                };

                //  Save old values to history log
                await _historyLogService.LogCustomerHistoryAsync(oldCustomer, customer.ModifiedBy ?? 0);

                //  Now update with new values
                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.FullName = customer.FullName;
                existingCustomer.ContactNo = customer.ContactNo;
                existingCustomer.WhatsAppNo = customer.WhatsAppNo;
                existingCustomer.EmailAddress = customer.EmailAddress;
                existingCustomer.CustomerTypeId = customer.CustomerTypeId;
                existingCustomer.CustomerIdentificationTypeId = customer.CustomerIdentificationTypeId;
                existingCustomer.IdentificationNo = customer.IdentificationNo;
                existingCustomer.GenderTypeId = customer.GenderTypeId;
                existingCustomer.DateOfBirth = customer.DateOfBirth;
                existingCustomer.RecordStatusId = customer.RecordStatusId;
                existingCustomer.ModifiedBy = customer.ModifiedBy;
                existingCustomer.ModifiedDate = DateTime.UtcNow;

                return await _repository.UpdateAsync(existingCustomer);
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

   