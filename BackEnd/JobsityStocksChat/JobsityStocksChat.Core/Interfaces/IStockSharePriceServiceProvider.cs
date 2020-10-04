using JobsityStocksChat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IStockSharePriceServiceProvider
    {
        StockSharePrice GetStockSharePrice(string stockCode);
    }
}
