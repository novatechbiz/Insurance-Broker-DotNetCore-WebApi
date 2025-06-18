using InsuraNova.Helpers;
using InsuraNova.Repositories;
using InsuraNova.Resources;

namespace InsuraNova.Services
{

    public interface ISystemFunctionService
    {
        Task<IEnumerable<SystemFunction>> GetSystemFunctionsAsync();
        Task<SystemFunction> GetSystemFunctionByIdAsync(int id);
        Task<SystemFunction> AddSystemFunctionAsync(SystemFunction systemFunction);
        Task<SystemFunction> UpdateSystemFunctionAsync(SystemFunction systemFunction);
        Task<bool> DeleteSystemFunctionAsync(int id);
    }

    public class SystemFunctionService : ISystemFunctionService
    {
        private readonly IRepository<SystemFunction> _repository;

        public SystemFunctionService(IRepository<SystemFunction> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SystemFunction>> GetSystemFunctionsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SystemFunction> GetSystemFunctionByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<SystemFunction> AddSystemFunctionAsync(SystemFunction systemFunction)
        {
            return await _repository.AddAsync(systemFunction);
        }

        public async Task<SystemFunction> UpdateSystemFunctionAsync(SystemFunction systemFunction)
        {
            return await _repository.UpdateAsync(systemFunction);
        }

        public async Task<bool> DeleteSystemFunctionAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }


}
