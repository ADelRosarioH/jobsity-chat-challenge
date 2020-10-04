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
        public ChatHub(IAsyncRepository<UserMessage> userMessageRepository, UserManager<ApplicationUser> userManager)
        {
            _userMessageRepository = userMessageRepository;
            _userManager = userManager;
        }

        public async Task SendMessage(ChatMessageViewModel message)
        {
            string userName = Context.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var createdAt = DateTime.Now;

            await _userMessageRepository.AddAsync(new UserMessage
            {
                Message = message.Message,
                UserId = user.Id,
                CreatedAt = createdAt
            });

            message.CreatedAt = createdAt;

            await Clients.All.SendAsync("RecieveMessage", message);
        }
    }
}
