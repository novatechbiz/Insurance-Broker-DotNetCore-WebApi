using InsuraNova.Repositories;

public interface ICustomerHistoryLogService
{
    Task LogCustomerHistoryAsync(CustomerHistoryLog oldCustomer, int modifiedBy);
}

public class CustomerHistoryLogService : ICustomerHistoryLogService
{
    private readonly IRepository<CustomerHistoryLog> _repository;

    public CustomerHistoryLogService(IRepository<CustomerHistoryLog> repository)
    {
        _repository = repository;
    }

    public async Task LogCustomerHistoryAsync(CustomerHistoryLog oldCustomer, int modifiedBy)
    {
        var log = new CustomerHistoryLog
        {
            CustomerId = oldCustomer.CustomerId,
            CompanyId = oldCustomer.CompanyId,
            CustomerIdentificationTypeId = oldCustomer.CustomerIdentificationTypeId,
            CustomerTypeId = oldCustomer.CustomerTypeId,
            CustomerName = oldCustomer.CustomerName,
            IdentificationNo = oldCustomer.IdentificationNo,
            FullName = oldCustomer.FullName,
            ContactNo = oldCustomer.ContactNo,
            WhatsAppNo = oldCustomer.WhatsAppNo,
            EmailAddress = oldCustomer.EmailAddress,
            GenderTypeId = oldCustomer.GenderTypeId,
            DateOfBirth = oldCustomer.DateOfBirth,
            RecordStatusId = oldCustomer.RecordStatusId,
            EnteredBy = oldCustomer.EnteredBy,
            EnteredDate = oldCustomer.EnteredDate,
            ModifiedBy = modifiedBy,
            ModifiedDate = DateTime.UtcNow
        };

        await _repository.AddAsync(log);
    }
}
