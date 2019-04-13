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

        public override void GetText(string text)
        {
            text = GetVerbatim(text);
            if (text.Contains("has been received from"))
            {
                Console.WriteLine(text);
                text = GetVerbatim(text);
                String[] elements = text.Split(' ');
                String amt = elements[0];
                String name = text.Substring(text.IndexOf(">") + 2, text.Length - text.IndexOf(">") - 2);
                // int amount = Int32.Parse(amt.Substring(1, amt.Length));
                save(amt.Substring(1) + " " + name);
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
