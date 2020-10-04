using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Identity;
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
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUserMessageAsyncRepository _repository;

        public ChatMessageService(IUserMessageAsyncRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<UserMessage>> GetLast50MessagesAsync()
        {
            return await _repository.GetLast50MessagesAsync();
        }

        public Task<StockShareInfo> GetStockShareInfoByCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}
