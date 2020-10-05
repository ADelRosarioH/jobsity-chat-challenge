using JobsityStocksChat.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.Infrastructure.Services
{
    public class ChatCommandHandler : IChatCommandHandler
    {
        public void Execute(string text, Action<string, string> action)
        {
            string commandWithArguments = text.Trim().Substring(1).ToLower();

            string[] parts = commandWithArguments.Split("=");

            string command = string.Empty;
            string args = string.Empty;

            if (parts.Length < 2)
            {
                command = parts[0];
            }
            else
            {
                command = parts[0];
                args = parts[1];
            }

            action(command, args);
        }

        public bool IsCommand(string text)
        {
            if (text.StartsWith("/") && text.Length > 1) return true;

            return false;
        }
    }
}
