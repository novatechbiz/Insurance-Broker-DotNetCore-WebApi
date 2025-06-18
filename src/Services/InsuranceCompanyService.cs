using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IInsuranceCompanyService
    {
        Task<IEnumerable<InsuranceCompany>> GetInsuranceCompaniesAsync();
        Task<InsuranceCompany> GetInsuranceCompanyByIdAsync(int id);
        Task<InsuranceCompany> AddInsuranceCompanyAsync(InsuranceCompany insuranceCompany);
        Task<InsuranceCompany> UpdateInsuranceCompanyAsync(InsuranceCompany insuranceCompany);
        Task<bool> DeleteInsuranceCompanyAsync(int id);
    }

    public class InsuranceCompanyService : IInsuranceCompanyService
    {
        private readonly IRepository<InsuranceCompany> _repository;

        public InsuranceCompanyService(IRepository<InsuranceCompany> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<InsuranceCompany>> GetInsuranceCompaniesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<InsuranceCompany> GetInsuranceCompanyByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<InsuranceCompany> AddInsuranceCompanyAsync(InsuranceCompany insuranceCompany)
        {
            return await _repository.AddAsync(insuranceCompany);
        }

        public async Task<InsuranceCompany> UpdateInsuranceCompanyAsync(InsuranceCompany insuranceCompany)
        {
            return await _repository.UpdateAsync(insuranceCompany);
        }

        public async Task<bool> DeleteInsuranceCompanyAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
