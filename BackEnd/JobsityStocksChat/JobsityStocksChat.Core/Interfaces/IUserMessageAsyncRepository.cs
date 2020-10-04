using JobsityStocksChat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IUserMessageAsyncRepository
    {
        Task<IReadOnlyList<UserMessage>> GetLast50MessagesAsync();
    }
}
