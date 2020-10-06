using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.UnitTests
{
    [TestFixture]
    public class ChatCommandHanlderTests
    {
        private IChatCommandHandler chatCommandHandler;

        [SetUp]
        protected void SetUp()
        {
            chatCommandHandler = new ChatCommandHandler();
        }

        [Test]
        public void Should_ReturnTrue_When_TextStartsWithSlash()
        {
            string text = "/stock";
            bool isCommand = chatCommandHandler.IsCommand(text);
            Assert.IsTrue(isCommand);
        }

        [Test]
        public void Should_ReturnFalse_When_TextStartsWithSlash()
        {
            string text = "simple message";
            bool isCommand = chatCommandHandler.IsCommand(text);
            Assert.IsFalse(isCommand);
        }

        [Test]
        public void Should_ParseOnlyCommand_When_TextIsCommand_And_HasNoArguments()
        {
            string text = "/stock";
            chatCommandHandler.Execute(text, (command, args) => { 
                Assert.IsTrue(command == "stock");
                Assert.IsTrue(string.IsNullOrWhiteSpace(args));
            });
        }

        [Test]
        public void Should_ParseCommandAndAruments_When_TextIsCommand()
        {
            string text = "/stock=stock_code";
            chatCommandHandler.Execute(text, (command, args) => {
                Assert.IsTrue(command == "stock");
                Assert.IsTrue(args == "stock_code");
            });
        }
    }
}
