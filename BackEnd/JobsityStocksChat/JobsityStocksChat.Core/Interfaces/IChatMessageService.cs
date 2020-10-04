using JobsityStocksChat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IChatMessageService
    {
        Task<IReadOnlyList<UserMessage>> GetLast50MessagesAsync();
        Task<StockShareInfo> GetStockShareInfoByCode(string code);
    }
}
