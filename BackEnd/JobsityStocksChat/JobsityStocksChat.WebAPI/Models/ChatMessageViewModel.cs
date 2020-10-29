using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityStocksChat.WebAPI.Models
{
    public class ChatMessageViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
