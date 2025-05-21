using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(int id);
        Task<Company> AddCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<bool> DeleteCompanyAsync(int id);
    }
    public class CompanyService(IRepository<Company> repository) : ICompanyService
    {
        private readonly IRepository<Company> _repository = repository;

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCompaniesMessage, ex);
            }
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToRetrieveCompanyByIdMessage, ex);
            }
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            try
            {
                return await _repository.AddAsync(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToAddCompanyMessage, ex);
            }
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            try
            {
                return  await _repository.UpdateAsync(company); 
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToUpdateCompanyMessage, ex);
            }
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ApplicationMessages.FailedToDeleteCompanyMessage, ex);
            }
        }
    }
}
