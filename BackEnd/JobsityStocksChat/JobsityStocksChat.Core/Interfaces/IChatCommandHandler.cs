using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Core.Interfaces
{
    public interface IChatCommandHandler
    {
        bool IsCommand(string text);
        void Execute(string text, Action<string, string> action);
    }
}
