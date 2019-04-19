using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This bot stops restarting.
    /// </summary>

    public class RestartHandler : ChatBot
    {
        private string[] phrases;

        /// <summary>
        /// This bot stops restarting.
        /// </summary>

        public RestartHandler(string[] phrases)
        {
            this.phrases = phrases;
        }

        public override bool OnDisconnect(DisconnectReason reason, string message)
        {
            foreach (string phrase in this.phrases)
            {
                if (message.Contains(phrase))
                {
                    Settings.restart = false;
                }
            }

            return false;
        }
    }
}
