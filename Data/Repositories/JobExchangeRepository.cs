using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class JobExchangeRepository : IJobExchangeRepository
    {
        private JobExchangeDBContext _jobExchangeDBContext;
        public JobExchangeRepository(JobExchangeDBContext jobExchangeDBContext)
        {
            _jobExchangeDBContext = jobExchangeDBContext;
        }

        public async Task<List<Сompany>> GetCompaniesAsync()
        {
            try
            {
                return await _jobExchangeDBContext.Companies.ToListAsync();
            }
            catch(Exception e) {
                return default;
            }
        }
    }
}
