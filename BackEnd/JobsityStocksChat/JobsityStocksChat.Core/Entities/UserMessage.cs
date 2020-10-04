using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }
}
