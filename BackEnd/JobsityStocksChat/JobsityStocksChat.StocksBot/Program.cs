using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Services;
using JobsityStocksChat.StocksBot.MQ;
using System;
using System.Threading;

namespace JobsityStocksChat.StocksBot
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            StockPriceResponseQueueProducer producer = new StockPriceResponseQueueProducer();
            StockPriceHandler priceHandler = new StockPriceHandler();

            StockPriceRequestQueueConsumer consumer = new StockPriceRequestQueueConsumer(producer, priceHandler);
            consumer.Start();

            Console.WriteLine("Waiting for incoming messages...");

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
            _closing.WaitOne();
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}
