﻿using JobsityStocksChat.Core.Constants;
using JobsityStocksChat.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.WebAPI.MQ
{
    public class StockPriceQueueProducer : IStockPriceQueueProducer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IConfiguration _configuration;

        public StockPriceQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            string hostName = _configuration.GetConnectionString("StocksSharePriceQueueConnection");

            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory() { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void RequestStockPrice(string stockCode)
        {
            _channel.QueueDeclare(queue: MQConstants.STOCK_PRICE_QUEUE_REQUEST,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(stockCode);

            _channel.BasicPublish(exchange: "", routingKey: MQConstants.STOCK_PRICE_QUEUE_REQUEST, body: messageBody, basicProperties: null);
        }
    }
}
