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

            if (parts.Length < 2) throw new ArgumentException("Command with no arguments.");

            string command = parts[0];

            string args = parts[1];

            action(command, args);
        }

        public bool IsCommand(string text)
        {
            if (text.StartsWith("/")) return true;

            return false;
        }
    }
}
