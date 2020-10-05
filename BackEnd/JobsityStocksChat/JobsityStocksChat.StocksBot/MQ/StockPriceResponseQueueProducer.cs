using JobsityStocksChat.Core.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.StocksBot.MQ
{
    public class StockPriceResponseQueueProducer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        public StockPriceResponseQueueProducer()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendStockInfo(StockShareInfo stockShareInfo)
        {
            _channel.QueueDeclare(queue: "STOCK_PRICE_QUEUE_RESPONSE",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string stockString = JsonConvert.SerializeObject(stockShareInfo);

            var body = Encoding.UTF8.GetBytes(stockString);

            _channel.BasicPublish(exchange: "",
                                 routingKey: "STOCK_PRICE_QUEUE_RESPONSE",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("[x] Sent {0}", stockString);

        }
    }
}
