using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ChatBot
{
    public interface IBot
    {
        string ExecuteCommand(string command, string arguments);
        bool isCommandSupported(string command);
        string GetBotName();
    }
}
