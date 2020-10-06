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
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
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
                    string response = $"{stockShareInfo.Symbol.ToUpper()} quote is {stockShareInfo.Close:F} per share";
                    await AnswerRequest(response);
                }
                else
                {
                    // send message to chat
                    string response = "I'm sorry, I couldn't find the stock your asked for.";
                    await AnswerRequest(response);
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

        private async Task AnswerRequest(string response)
        {
            await _chatHub.Clients.All.SendAsync("RecieveMessage", new ChatMessageViewModel
            {
                UserName = "StockBot",
                Message = response,
                CreatedAt = DateTime.Now
            });
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
