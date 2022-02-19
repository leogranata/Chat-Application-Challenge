using Common.ChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager
{
    public class BotManager
    {
        Dictionary<string, IBot> bots = new Dictionary<string, IBot>();
        public BotManager()
        {
            Stock.ChatBot.StockChatBot stockChatBot = new Stock.ChatBot.StockChatBot();

            RegisterBot(stockChatBot.GetBotName(), stockChatBot);
        }

        public string ExecuteCommand (string commandMessage)
        {
            string response = null;
            string commandName = ExtractCommandName(commandMessage);
            string commandArgs = ExtractCommandArguments(commandMessage);

            foreach (IBot bot in bots.Values)
            {
                if (bot.IsCommandSupported(commandName))
                {
                    response = "<b>" + bot.GetBotName() + "</b>: " + bot.ExecuteCommand(commandName, commandArgs);
                }
            }
            return response;
        }

        private string ExtractCommandName (string command)
        {
            return command.Split(new string[] { "/" }, StringSplitOptions.None)[1]
                    .Split('=')[0]
                    .Trim();
        }
        private string ExtractCommandArguments(string command)
        {
            return command.Split('=')[1].Trim();
        }

        private void RegisterBot (string botName, IBot bot)
        {
            if (!bots.ContainsKey(botName))
            {
                bots.Add(botName, bot);
            }
        }
    }

    
}
