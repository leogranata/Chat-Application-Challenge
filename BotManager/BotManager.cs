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
        private Dictionary<string, IBot> bots = new Dictionary<string, IBot>();
        public BotManager()
        {
            // Get chatbots from configuration
            string chatBots = Properties.Settings.Default.ChatBots;
            string[] chatbotList = chatBots.Split(',');
            foreach (string botClass in chatbotList)
            {
                // Create instance of bot
                var type = Type.GetType("botClass");
                var chatObject = (IBot)Activator.CreateInstance(type);
                RegisterBot(chatObject.GetBotName(), chatObject);
            }
            
        }

        public string ExecuteCommand (string commandMessage)
        {
            string response = string.Empty;
            string commandName = ExtractCommandName(commandMessage);
            string commandArgs = ExtractCommandArguments(commandMessage);

            foreach (IBot bot in bots.Values)
            {
                if (bot.isCommandSupported(commandName))
                {
                    response = "<b>" + bot.GetBotName() + ":" + bot.ExecuteCommand(commandName, commandArgs);
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
