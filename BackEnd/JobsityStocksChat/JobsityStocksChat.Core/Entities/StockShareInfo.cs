using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Entities
{
    public class StockShareInfo
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}
