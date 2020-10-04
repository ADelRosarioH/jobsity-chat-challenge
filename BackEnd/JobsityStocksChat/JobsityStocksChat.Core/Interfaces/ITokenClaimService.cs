using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface ITokenClaimService
    {
        string GetToken(string userName);
    }
}
