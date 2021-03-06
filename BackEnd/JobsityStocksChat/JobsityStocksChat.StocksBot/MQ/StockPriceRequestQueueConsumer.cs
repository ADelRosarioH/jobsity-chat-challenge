﻿using JobsityStocksChat.Core.Constants;
using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using Microsoft.Extensions.Configuration;
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

        protected readonly IConfiguration _configuration;


        public StockPriceRequestQueueConsumer(IConfiguration configuration, StockPriceResponseQueueProducer producer, IStockPriceHandler priceHandler)
        {

            _configuration = configuration;
            string hostName = _configuration.GetConnectionString("StocksSharePriceQueueConnection");

            _producer = producer;
            _priceHandler = priceHandler;

            _factory = new ConnectionFactory() { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Start()
        {

            _channel.QueueDeclare(queue: MQConstants.STOCK_PRICE_QUEUE_REQUEST,
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

            _channel.BasicConsume(queue: MQConstants.STOCK_PRICE_QUEUE_REQUEST,
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
