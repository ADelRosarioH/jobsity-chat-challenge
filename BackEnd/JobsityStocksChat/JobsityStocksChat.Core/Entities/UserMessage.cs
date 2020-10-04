using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Entities
{
    public partial class UserMessage
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
