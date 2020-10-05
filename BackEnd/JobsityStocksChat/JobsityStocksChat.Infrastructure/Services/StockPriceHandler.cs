using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace JobsityStocksChat.Infrastructure.Services
{
    public class StockPriceHandler : IStockPriceHandler
    {
        public async Task<StockShareInfo> GetStockShareInfo(string stockCode)
        {
            // https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv
            string url = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            HttpClient client = new HttpClient();
            Stream content = await client.GetStreamAsync(url);

            CsvParser<StockShareInfo> csvParser = new CsvParser<StockShareInfo>(new CsvParserOptions(true, ','), 
                new StockShareInfoCsvMapping());

            var results = csvParser.ReadFromStream(stream: content, Encoding.UTF8).ToList();

            return results.FirstOrDefault()?.Result;
        }
    }

    public class StockShareInfoCsvMapping : CsvMapping<StockShareInfo>
    {
        public StockShareInfoCsvMapping() : base()
        {
            MapProperty(0, t => t.Symbol);
            MapProperty(1, t => t.Date);
            MapProperty(2, t => t.Time);
            MapProperty(3, t => t.Open);
            MapProperty(4, t => t.High);
            MapProperty(5, t => t.Low);
            MapProperty(6, t => t.Close);
            MapProperty(7, t => t.Volume);
        }
    }
}