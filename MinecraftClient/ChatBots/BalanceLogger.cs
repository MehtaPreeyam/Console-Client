using System;
using System.IO;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This bot logs the players in players.txt.
    /// </summary>

    public class BalanceLogger : ChatBot
    {
        private int count;
        private int interval;
        private string[] players;
        private string logfile;
        private string playerfile;

        /// <summary>
        /// This bot logs balances of players in players.txt.
        /// </summary>
        /// <param name="interval">Time amount between each log (10 = 1s, 600 = 1 minute, etc.)</param>

        public BalanceLogger(int interval, string playerfile, string logfile)
        {
            this.interval = interval;
            this.playerfile = playerfile;
            this.logfile = logfile;
            this.players = File.ReadAllLines(this.playerfile);
        }

        public override void GetText(string text)
        {
            text = GetVerbatim(text);
            if (text.Contains("'s Balance"))
            {
                save(text);
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
            if (count == interval)
            {
                for(int i = 0; i < players.Length; i++)
                {
                    SendText("/bal " + players[i]);
                }

                count = 0;
            }
        }
    }
}
