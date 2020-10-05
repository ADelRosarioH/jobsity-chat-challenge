using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.WebAPI.Hubs;
using JobsityStocksChat.WebAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobsityStocksChat.WebAPI.MQ
{
    public class StockPriceQueueConsumer : BackgroundService, IStockPriceQueueConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IHubContext<ChatHub> _chatHub;

        public StockPriceQueueConsumer(IHubContext<ChatHub> chatHub)
        {
            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "STOCK_PRICE_QUEUE_RESPONSE",
                durable: false,
                exclusive: false,
                autoDelete: false);

            _chatHub = chatHub;
        }


        public virtual void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockShareInfo = JsonConvert.DeserializeObject<StockShareInfo>(message);

                if (stockShareInfo != null )
                {
                    // send message to chat
                    await _chatHub.Clients.All.SendAsync("RecieveMessage", new ChatMessageViewModel
                    {
                        UserName = "StockBot",
                        Message = $"Stock: {stockShareInfo.Symbol} High: {stockShareInfo.High} Low: {stockShareInfo.Low}",
                        CreatedAt = DateTime.Now
                    });
                }
                else
                {
                    // send message to chat
                    await _chatHub.Clients.All.SendAsync("RecieveMessage", new ChatMessageViewModel
                    {
                        UserName = "StockBot",
                        Message = "I'm sorry, I couldn't find the stock your asked for.",
                        CreatedAt = DateTime.Now
                    });
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: "STOCK_PRICE_QUEUE_RESPONSE", autoAck: false, consumer: consumer);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            Start();

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
