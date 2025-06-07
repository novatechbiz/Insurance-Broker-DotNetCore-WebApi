using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICompanyTypeService
    {
        Task<IEnumerable<CompanyType>> GetCompanyTypesAsync();
        Task<CompanyType> GetCompanyTypeByIdAsync(int id);
        Task<CompanyType> AddCompanyTypeAsync(CompanyType companyType);
        Task<CompanyType> UpdateCompanyTypeAsync(CompanyType companyType);
        Task<bool> DeleteCompanyTypeAsync(int id);
    }

    public class CompanyTypeService : ICompanyTypeService
    {
        private readonly IRepository<CompanyType> _repository;

        public CompanyTypeService(IRepository<CompanyType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CompanyType>> GetCompanyTypesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CompanyType> GetCompanyTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<CompanyType> AddCompanyTypeAsync(CompanyType companyType)
        {
            return await _repository.AddAsync(companyType);
        }

        public async Task<CompanyType> UpdateCompanyTypeAsync(CompanyType companyType)
        {
            return await _repository.UpdateAsync(companyType);
        }

        public async Task<bool> DeleteCompanyTypeAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }

}
