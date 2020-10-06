using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Services;
using JobsityStocksChat.StocksBot.MQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;

namespace JobsityStocksChat.StocksBot
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // reads configs
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            // setup queue message handlers
            StockPriceResponseQueueProducer producer = new StockPriceResponseQueueProducer(configuration);
            StockPriceHandler priceHandler = new StockPriceHandler();

            StockPriceRequestQueueConsumer consumer = new StockPriceRequestQueueConsumer(configuration, producer, priceHandler);

            // starts listening
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
