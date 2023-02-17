using Data.Models;

namespace Data.Interfaces
{
    public interface IJobExchangeRepository
    {
        public Task<List<Сompany>> GetCompaniesAsync();
    }
}
