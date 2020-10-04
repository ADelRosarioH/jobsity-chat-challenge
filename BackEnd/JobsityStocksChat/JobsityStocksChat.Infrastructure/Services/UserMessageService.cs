using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Infrastructure.Services
{
    public class UserMessageService : IAsyncRepository<UserMessage>
    {
        private readonly ApplicationDbContext _context;

        public UserMessageService()
        {

        }

        public Task<UserMessage> AddAsync(UserMessage entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(UserMessage entity)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> FirstOrDefaultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<UserMessage>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserMessage entity)
        {
            throw new NotImplementedException();
        }
    }
}
