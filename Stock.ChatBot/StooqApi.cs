using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stock.ChatBot
{
    public static class StooqApi
    {
        const string stooqUrlTemplate = "https://stooq.com/q/l/?s=[SYMBOL]&f=sd2t2ohlcv&h&e=csv";
        public static string GetData(string symbol)
        {
            string requestUrl = stooqUrlTemplate.Replace("[SYMBOL]", symbol);

            try
            {
                WebRequest request = HttpWebRequest.Create(requestUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseText = reader.ReadToEnd();
                string[] responseElements = responseText.Split(',');
                return string.Format("{0} quote is ${1} per share", symbol.ToUpper(), responseElements[13]);
            }
            catch (Exception)
            {
                return string.Format("Error Trying to call Stock Market API with URL {0}", requestUrl);
            }
        }

    }
}
