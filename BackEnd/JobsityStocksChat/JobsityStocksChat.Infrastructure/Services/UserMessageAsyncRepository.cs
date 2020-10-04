using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Infrastructure.Services
{
    public class UserMessageAsyncRepository : IAsyncRepository<UserMessage>, IUserMessageAsyncRepository
    {
        private readonly ApplicationDbContext _context;

        public UserMessageAsyncRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserMessage> AddAsync(UserMessage entity)
        {
            _context.Messages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(UserMessage entity)
        {
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserMessage> FirstOrDefaultAsync()
        {
            return await _context.Messages.FirstOrDefaultAsync();
        }

        public async Task<UserMessage> GetByIdAsync(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<UserMessage>> GetLast50MessagesAsync()
        {
            return await _context.Messages.Include(t => t.User)
                .OrderBy(t => t.CreatedAt)
                //.TakeLast(50)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<UserMessage>> ListAllAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task UpdateAsync(UserMessage entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
