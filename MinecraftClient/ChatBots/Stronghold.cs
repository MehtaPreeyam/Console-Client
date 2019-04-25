using System;
using System.Collections.Generic;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This alerts ally chat when stronghold is getting lost
    /// </summary>

    public class Stronghold : ChatBot
    {
        private int count;
        private int timeping;
        private double sh_percent = 100.0;

        /// <summary>
        /// This alerts ally chat when stronghold is getting lost
        /// </summary>

        public override void GetText(string text)
        {
            text = GetVerbatim(text);

            if (text.Contains("BadRep is no longer controlling Frozen"))
            {
                Protocol.Handlers.Protocol18Handler.sh = false;
                Protocol.Handlers.Protocol18Handler.sh_percent = 0.0;
                this.sh_percent = 100.0;
            }
        }
        public Stronghold(int timeping)
        {
            count = 0;
            this.timeping = timeping;
        }
        public override void Update()
        {
            if (Protocol.Handlers.Protocol18Handler.sh)
            {
                if(Protocol.Handlers.Protocol18Handler.sh_percent < this.sh_percent)
                {
                    SendText("Stronghold has dropped to: " + Protocol.Handlers.Protocol18Handler.sh_percent + "%");
                }
            }

            this.sh_percent = Protocol.Handlers.Protocol18Handler.sh_percent;

            count++;
            if (count == timeping)
            {
                SendText("/sh");
                count = 0;
            }
        }
    }
}
