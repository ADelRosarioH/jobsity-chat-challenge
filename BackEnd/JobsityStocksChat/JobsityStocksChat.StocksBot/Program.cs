using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Services;
using JobsityStocksChat.StocksBot.MQ;
using System;

namespace JobsityStocksChat.StocksBot
{
    class Program
    {
        static void Main(string[] args)
        {
            StockPriceResponseQueueProducer producer = new StockPriceResponseQueueProducer();
            StockPriceHandler priceHandler = new StockPriceHandler();

            StockPriceRequestQueueConsumer consumer = new StockPriceRequestQueueConsumer(producer, priceHandler);

            consumer.Start();

            Console.WriteLine("Waiting for incoming messages...");
            Console.ReadKey();
        }
    }
}
