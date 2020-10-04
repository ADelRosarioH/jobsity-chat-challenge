using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserMessage> Messages { get; set; }
    }
}
