﻿using JobsityStocksChat.Core.Constants;
using JobsityStocksChat.Core.Entities;
using Microsoft.Extensions.Configuration;
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
        protected readonly IConfiguration _configuration;

        public StockPriceResponseQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            string hostName = _configuration.GetConnectionString("StocksSharePriceQueueConnection");

            _factory = new ConnectionFactory() { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendStockInfo(StockShareInfo stockShareInfo)
        {
            _channel.QueueDeclare(queue: MQConstants.STOCK_PRICE_QUEUE_RESPONSE,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string stockString = JsonConvert.SerializeObject(stockShareInfo);

            var body = Encoding.UTF8.GetBytes(stockString);

            _channel.BasicPublish(exchange: "",
                                 routingKey: MQConstants.STOCK_PRICE_QUEUE_RESPONSE,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("[x] Sent {0}", stockString);

        }
    }
}
