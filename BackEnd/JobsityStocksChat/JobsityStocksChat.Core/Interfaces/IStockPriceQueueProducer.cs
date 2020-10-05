using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IStockPriceQueueProducer
    {
        void RequestStockPrice(string stockCode);
    }
}
