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
        public StockChatBot()
        {

        }

        public string ExecuteCommand(string command, string arguments)
        {
            return StooqApi.GetData(arguments);
        }

        public string GetBotName()
        {
            return "Stock Bot";
        }

        public bool IsCommandSupported(string command)
        {
            if (command == "stock") return true;
            return false;
        }
    }
}
