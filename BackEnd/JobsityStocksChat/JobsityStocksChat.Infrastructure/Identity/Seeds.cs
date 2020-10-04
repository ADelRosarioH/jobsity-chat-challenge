using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Infrastructure.Identity
{
    public static class Seeds
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "demo@jobsity.io", Email = "demo@jobsity.io" };
            await userManager.CreateAsync(defaultUser, "P@55w0rd");
        }
    }
}
