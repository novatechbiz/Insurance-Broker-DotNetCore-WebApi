using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{
    public interface IPremiumLineService
    {
        Task<IEnumerable<PremiumLine>> GetPremiumLinesAsync();
        Task<PremiumLine> GetPremiumLineByIdAsync(int id);
        Task<PremiumLine> AddPremiumLineAsync(PremiumLine premiumLine);
        Task<PremiumLine> UpdatePremiumLineAsync(PremiumLine premiumLine);
        Task<bool> DeletePremiumLineAsync(int id);
    }

    public class PremiumLineService : IPremiumLineService
    {
        private readonly IRepository<PremiumLine> _repository;

        public PremiumLineService(IRepository<PremiumLine> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PremiumLine>> GetPremiumLinesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PremiumLine> GetPremiumLineByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PremiumLine> AddPremiumLineAsync(PremiumLine premiumLine)
        {
            return await _repository.AddAsync(premiumLine);
        }

        public async Task<PremiumLine> UpdatePremiumLineAsync(PremiumLine premiumLine)
        {
            return await _repository.UpdateAsync(premiumLine);
        }

        public async Task<bool> DeletePremiumLineAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
