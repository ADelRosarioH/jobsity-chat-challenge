using JobsityStocksChat.Core.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityStocksChat.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }

        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }
    }
}
