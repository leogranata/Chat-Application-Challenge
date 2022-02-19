using Common.ChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.ChatBot
{
    public class StockChatBot : IBot
    {
        public string ExecuteCommand(string command, string arguments)
        {
            return "this comes from the BOT - Command:" + command + " arguments:" + arguments;
        }

        public string GetBotName()
        {
            return "Stock Bot";
        }

        public bool isCommandSupported(string command)
        {
            if (command == "test") return true;
            return false;
        }
    }
}
