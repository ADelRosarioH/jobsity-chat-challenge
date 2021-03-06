﻿using JobsityStocksChat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IStockPriceHandler
    {
        Task<StockShareInfo> GetStockShareInfo(string stockCode);
    }
}
