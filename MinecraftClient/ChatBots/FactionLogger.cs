using System;
using System.IO;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This bot logs the players in players.txt.
    /// </summary>

    public class FactionLogger : ChatBot
    {
        private int count;
        private int interval;
        private string logfile = "FactionLog.txt";
        private string facfile = "Factions.txt";
        private string[] Factions;
        private string currFac;
        private int index = 0;
        private int timeBetween = 600;

        /// <summary>
        /// This bot logs balances of players in players.txt.
        /// </summary>
        /// <param name="interval">Time amount between each log (10 = 1s, 600 = 1 minute, etc.)</param>

        public FactionLogger(int interval)
        {
            this.interval = interval;
            string directory = Path.GetDirectoryName(facfile);
            if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            this.Factions = File.ReadAllLines(facfile);
            this.currFac = Factions[0];
        }

        public override void GetText(string text)
        {
            text = GetVerbatim(text);
            if (text.Contains("Members online:"))
            {
                Console.WriteLine(text);
                int numOnline = text.Length - text.Replace(",", "").Length+1;
                save(currFac + " " + numOnline);
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

        public override void Update()
        {
            count++;
            if (index == 0)
            {
                if (count == interval)
                {
                    currFac = Factions[index];
                    SendText("/f who " + Factions[index]);
                    if (index == Factions.Length - 1)
                    {
                        index = 0;
                        count = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            else
            {
                if(count == interval + (timeBetween * index))
                {
                    currFac = Factions[index];
                    SendText("/f who " + Factions[index]);
                    if (index == Factions.Length - 1)
                    {
                        index = 0;
                        count = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }
    }
}
