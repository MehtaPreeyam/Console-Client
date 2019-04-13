using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This bot saves the received messages in a text file.
    /// </summary>

    public class MoneyBot : ChatBot
    {
        private string logfile = "MoneyLog.txt";

        public MoneyBot()
        {
            if (String.IsNullOrEmpty(logfile) || logfile.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                LogToConsole("Path '" + logfile + "' contains invalid characters.");
                UnloadBot();
            }
        }
        public string judgePayment(int amt)
        {
            if(amt < 100000000)
            {
                return " Oof, less than 100 mil sell? Are you sure you're not holding out on us?";
            }
            else if(amt < 250000000)
            {
                return " Not a bad sell, but also not a great sell. Dont worry you'll get there one day";
            }
            else
            {
                return " Monster sell, good shit";
            }
        }

        public override void GetText(string text)
        {
            text = GetVerbatim(text);
            if (text.Contains("has been received from"))
            {
                text = GetVerbatim(text);
                String[] elements = text.Split(' ');
                String amt = elements[0];
                String name = text.Substring(text.IndexOf(">") + 2, text.Length - text.IndexOf(">") - 3);
                int amount = Int32.Parse(amt.Substring(1)) * 10;
                save(amt.Substring(1) + " " + name);
                string response = "/msg "+name+" Hello " + name + ". We thank you for your payment of " + amt + ".";
                SendText(response);
                SendText("/msg " + name + judgePayment(amount));
            }
        }

        private void save(string tosave)
        {
            tosave = GetTimestamp() + ' ' + tosave;
            string directory = Path.GetDirectoryName(logfile);
            if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            FileStream stream = new FileStream(logfile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(stream);
            stream.Seek(0, SeekOrigin.End);
            writer.WriteLine(tosave);
            writer.Dispose();
            stream.Close();
        }
    }
}
