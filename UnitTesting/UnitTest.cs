using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest
    {
        private const string symbol = "AMZN.US";
        private const string testCmd = "/stock=AMZN.US";
        [TestMethod]
        public void TestStockBot()
        {
            string response = Stock.ChatBot.StooqApi.GetData(symbol);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("quote is") && response.Contains("per share"));
        }

        [TestMethod]
        public void TestBotManager()
        {
            BotManager.BotManager mgr = new BotManager.BotManager();
            string response = mgr.ExecuteCommand(testCmd);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("quote is") && response.Contains("per share"));
        }

    }
}
