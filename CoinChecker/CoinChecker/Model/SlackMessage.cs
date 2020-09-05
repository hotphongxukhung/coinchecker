using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoinChecker.Model
{
    public class SlackMessage
    {

        public SlackMessage()
        {
        }

        public static void SendMessageToSlack(string content)
        {

            var s = "{\"text\": \"" + content + "\"}";
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var response = client.UploadData("https://hooks.slack.com/services/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", "POST", Encoding.UTF8.GetBytes(s));
            string responseInString = Encoding.UTF8.GetString(response);
            Console.WriteLine(responseInString);
        }
    }
}
