using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.StocksBot.MQ
{
    public class StockPriceRequestQueueConsumer
    {
        private readonly StockPriceResponseQueueProducer _producer;
        private readonly IStockPriceHandler _priceHandler;

        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        public StockPriceRequestQueueConsumer(StockPriceResponseQueueProducer producer, IStockPriceHandler priceHandler)
        {
            _producer = producer;
            _priceHandler = priceHandler;

            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Start()
        {

            _channel.QueueDeclare(queue: "STOCK_PRICE_QUEUE_REQUEST",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("[x] Received {0}", message);

                var stockInfo = await _priceHandler.GetStockShareInfo(message);

                var result = JsonConvert.SerializeObject(stockInfo);
                _producer.SendStockInfo(stockInfo);
            };

            _channel.BasicConsume(queue: "STOCK_PRICE_QUEUE_REQUEST",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
