using JobsityStocksChat.Core.Constants;
using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace JobsityStocksChat.WebAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IAsyncRepository<UserMessage> _userMessageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IChatCommandHandler _chatCommandHandler;
        private readonly IStockPriceQueueProducer _stockPriceQueueProducer;

        public ChatHub(IAsyncRepository<UserMessage> userMessageRepository,
            UserManager<ApplicationUser> userManager,
            IChatCommandHandler chatCommandHandler,
            IStockPriceQueueProducer stockPriceQueueProducer)
        {
            _userMessageRepository = userMessageRepository;
            _userManager = userManager;
            _chatCommandHandler = chatCommandHandler;
            _stockPriceQueueProducer = stockPriceQueueProducer;
        }

        public async Task SendMessage(ChatMessageViewModel message)
        {
            if (!_chatCommandHandler.IsCommand(message.Message))
            {
                await SaveMessageToDatabase(message);
            }
            else
            {
                _chatCommandHandler.Execute(message.Message, (command, args) =>
                {
                    if (command == "stock")
                    {
                        // send message to queue
                        _stockPriceQueueProducer.RequestStockPrice(args);
                    }
                });
            }

            await Clients.All.SendAsync(ChatHubConstants.CLIENT_METHOD_NAME, message);
        }

        private async Task SaveMessageToDatabase(ChatMessageViewModel message)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);

            await _userMessageRepository.AddAsync(new UserMessage
            {
                Message = message.Message,
                UserId = user.Id,
                CreatedAt = message.CreatedAt
            });
        }
    }
}
